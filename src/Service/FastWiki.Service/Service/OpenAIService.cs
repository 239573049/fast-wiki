using System.Text;
using System.Text.Json;
using FastWiki.Service.Application.Storage.Queries;
using FastWiki.Service.Contracts.OpenAI;
using FastWiki.Service.Domain.Storage.Aggregates;
using FastWiki.Service.Infrastructure;
using FastWiki.Service.Infrastructure.Helper;
using Microsoft.SemanticKernel.ChatCompletion;

namespace FastWiki.Service.Service;

public static class OpenAIService
{
    public static async Task Completions(HttpContext context)
    {
        using var stream = new StreamReader(context.Request.Body);

        var module =
            JsonSerializer.Deserialize<ChatCompletionDto<ChatCompletionRequestMessage>>(await stream.ReadToEndAsync());

        context.Response.ContentType = "text/event-stream";

        if (module == null)
        {
            await context.WriteEndAsync("Invalid request");

            return;
        }

        var chatDialogId = context.Request.Query["ChatDialogId"];
        var chatId = context.Request.Query["ChatId"];
        var token = context.Request.Headers.Authorization;
        var chatShareId = context.Request.Query["ChatShareId"];


        if (chatDialogId.IsNullOrEmpty())
        {
            await context.WriteEndAsync(nameof(chatDialogId) + "����Ϊ��");
            return;
        }

        if (chatId.IsNullOrEmpty())
        {
            await context.WriteEndAsync(nameof(chatId) + "����Ϊ��");
            return;
        }

        var eventBus = context.RequestServices.GetRequiredService<IEventBus>();

        var getAPIKeyChatShareQuery = new GetAPIKeyChatShareQuery(token);

        // �����sk-��ͷ��token����Ӧ��token
        if (chatShareId.IsNullOrEmpty() && !token.ToString().Replace("Bearer ", "").Trim().StartsWith("sk-"))
        {
            // �жϵ�ǰ�û��Ƿ��¼
            if (context.User.Identity?.IsAuthenticated == false)
            {
                context.Response.StatusCode = 401;
                await context.WriteEndAsync("Token����Ϊ��");
                return;
            }
        }
        else if (chatShareId.IsNullOrEmpty())
        {
            await eventBus.PublishAsync(getAPIKeyChatShareQuery);

            if (getAPIKeyChatShareQuery.Result == null)
            {
                context.Response.StatusCode = 401;
                await context.WriteEndAsync("Token��Ч");
                return;
            }
        }

        ChatApplicationDto chatApplication = null;

        if (!chatShareId.IsNullOrEmpty())
        {
            var chatShareInfoQuery = new ChatShareInfoQuery(chatShareId);

            await eventBus.PublishAsync(chatShareInfoQuery);

            var chatApplicationQuery = new ChatApplicationInfoQuery(chatShareInfoQuery.Result.ChatApplicationId);

            await eventBus.PublishAsync(chatApplicationQuery);

            chatApplication = chatApplicationQuery?.Result;
        }
        else
        {
            var chatApplicationQuery = new ChatApplicationInfoQuery(chatId);
            await eventBus.PublishAsync(chatApplicationQuery);
            chatApplication = chatApplicationQuery?.Result;
        }

        if (chatApplication == null)
        {
            await context.WriteEndAsync("Ӧ��Id������");
            return;
        }

        int requestToken = 0;

        var chatHistory = new ChatHistory();

        // ���������Prompt�������
        if (!chatApplication.Prompt.IsNullOrEmpty())
        {
            chatHistory.AddSystemMessage(chatApplication.Prompt);
        }

        // ����û����룬���Ҽ�������token����
        module.messages.ForEach(x =>
        {
            if (!x.content.IsNullOrEmpty())
            {
                requestToken += TokenHelper.ComputeToken(x.content);
                if (x.role == "user")
                {
                    chatHistory.AddUserMessage(x.content);
                }
                else if (x.role == "assistant")
                {
                    chatHistory.AddSystemMessage(x.content);
                }
                else if (x.role == "system")
                {
                    chatHistory.AddSystemMessage(x.content);
                }
            }
        });

        var content = module.messages.Last();

        var prompt = string.Empty;

        var sourceFile = new List<FileStorage>();
        // ���Ϊ����ʹ��֪ʶ��
        if (chatApplication.WikiIds.Count != 0)
        {
            var memoryServerless = context.RequestServices.GetRequiredService<MemoryServerless>();

            var filters = chatApplication.WikiIds
                .Select(chatApplication => new MemoryFilter().ByTag("wikiId", chatApplication.ToString())).ToList();

            var result = await memoryServerless.SearchAsync(content.content, "wiki", filters: filters, limit: 3,
                minRelevance: chatApplication.Relevancy);

            var fileIds = new List<long>();

            result.Results.ForEach(x =>
            {
                // ��ȡfileId
                var fileId = x.Partitions.Select(x => x.Tags.FirstOrDefault(x => x.Key == "fileId"))
                    .FirstOrDefault(x => !x.Value.IsNullOrEmpty())
                    .Value.FirstOrDefault();

                if (!fileId.IsNullOrWhiteSpace() && long.TryParse(fileId, out var id))
                {
                    fileIds.Add(id);
                }

                prompt += string.Join(Environment.NewLine, x.Partitions.Select(x => x.Text));
            });

            if (result.Results.Count == 0 &&
                !string.IsNullOrWhiteSpace(chatApplication.NoReplyFoundTemplate))
            {
                await context.WriteEndAsync(chatApplication.NoReplyFoundTemplate);
                return;
            }

            var tokens = TokenHelper.GetGptEncoding().Encode(prompt);

            // ���������Ч�ķ�ֹtoken�����������ƣ�����Ҳ�ή�ͻظ�������
            prompt = TokenHelper.GetGptEncoding()
                .Decode(tokens.Take(chatApplication.MaxResponseToken).ToList());

            // ���prompt��Ϊ�գ�����Ҫ����ģ���滻
            if (!prompt.IsNormalized())
            {
                content.content = chatApplication.Template.Replace("{{quote}}", prompt)
                    .Replace("{{question}}", content.content);
            }

            // ��������Ҫ��ȡԴ�ļ�
            if (fileIds.Count > 0 && chatApplication.ShowSourceFile)
            {
                var fileQuery = new StorageInfosQuery(fileIds);

                await eventBus.PublishAsync(fileQuery);

                sourceFile.AddRange(fileQuery.Result);
            }
        }

        if (getAPIKeyChatShareQuery?.Result != null)
        {
            // ���token�����򷵻أ�ʹ��token�͵�ǰrequest�ܺʹ��ڿ���token���򷵻�
            if (getAPIKeyChatShareQuery.Result.AvailableToken != -1 &&
                (getAPIKeyChatShareQuery.Result.UsedToken + requestToken) >=
                getAPIKeyChatShareQuery.Result.AvailableToken)
            {
                await context.WriteEndAsync("Token����");
                return;
            }

            // ���û�й��������
            if (getAPIKeyChatShareQuery.Result.Expires != null ||
                getAPIKeyChatShareQuery.Result.Expires < DateTimeOffset.Now)
            {
                await context.WriteEndAsync("Token�ѹ���");
                return;
            }
        }

        var wikiMemoryService = context.RequestServices.GetRequiredService<WikiMemoryService>();

        var chatStream = wikiMemoryService.CreateOpenAIChatCompletionService(chatApplication.ChatModel);

        var output = new StringBuilder();
        await foreach (var item in chatStream.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            if (item.Content.IsNullOrEmpty())
            {
                continue;
            }

            output.Append(item.Content);
            await context.WriteOpenAiResultAsync(item.Content, module.model, Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"));
        }

        await context.WriteEndAsync();

        #region ��¼�Ի�����

        var createChatDialogHistoryCommand = new CreateChatDialogHistoryCommand(new CreateChatDialogHistoryInput()
        {
            ChatDialogId = chatDialogId,
            Content = content.content,
            ExpendToken = requestToken,
            Type = ChatDialogHistoryType.Text,
            Current = true
        });

        await eventBus.PublishAsync(createChatDialogHistoryCommand);

        var outputContent = output.ToString();

        var chatDialogHistory = new CreateChatDialogHistoryCommand(new CreateChatDialogHistoryInput()
        {
            ChatDialogId = chatDialogId,
            Content = outputContent,
            ExpendToken = TokenHelper.ComputeToken(outputContent),
            Type = ChatDialogHistoryType.Text,
            Current = false,
            SourceFile = sourceFile.Select(x => new SourceFileDto()
            {
                Name = x.Name,
                FileId = x.Id.ToString(),
                FilePath = x.Path
            }).ToList()
        });

        await eventBus.PublishAsync(chatDialogHistory);

        #endregion
    }

    private static bool IsVision(string model)
    {
        if (model.Contains("vision") || model.Contains("image"))
        {
            return true;
        }

        return false;
    }
}