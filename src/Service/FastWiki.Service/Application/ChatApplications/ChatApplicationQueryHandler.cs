namespace FastWiki.Service.Application.ChatApplications;

public class ChatApplicationQueryHandler(
    IChatApplicationRepository chatApplicationRepository,
    IMapper mapper)
{
    [EventHandler]
    public async Task ChatApplicationAsync(ChatApplicationQuery query)
    {
        var result = await chatApplicationRepository.GetListAsync(query.Page, query.PageSize, query.userId);

        var total = await chatApplicationRepository.GetCountAsync(query.userId);

        query.Result = new PaginatedListBase<ChatApplicationDto>
        {
            Result = mapper.Map<List<ChatApplicationDto>>(result),
            Total = total
        };
    }

    [EventHandler]
    public async Task ChatApplicationInfoAsync(ChatApplicationInfoQuery query)
    {
        var result = await chatApplicationRepository.FindAsync(query.Id);

        query.Result = mapper.Map<ChatApplicationDto>(result);
    }

    [EventHandler]
    public async Task ChatShareAsync(ChatShareQuery query)
    {
        var result = await chatApplicationRepository.GetChatShareListAsync(query.userId,
            query.chatApplicationId, query.page, query.pageSize);

        var total = await chatApplicationRepository.GetChatShareCountAsync(query.userId,
            query.chatApplicationId);


        query.Result = new PaginatedListBase<ChatShareDto>
        {
            Result = mapper.Map<List<ChatShareDto>>(result),
            Total = total
        };
    }

    [EventHandler]
    public async Task ChatShareInfoAsync(ChatShareInfoQuery query)
    {
        var result = await chatApplicationRepository.GetChatShareAsync(query.Id);

        query.Result = mapper.Map<ChatShareDto>(result);
    }

    [EventHandler]
    public async Task ChatShareApplicationAsync(ChatShareApplicationQuery query)
    {
        var chatApplication = await chatApplicationRepository.ChatShareApplicationAsync(query.chatSharedId);

        query.Result = mapper.Map<ChatApplicationDto>(chatApplication);
    }

    [EventHandler]
    public async Task GetQuestionsAsync(GetQuestionsQuery query)
    {
        var result = await chatApplicationRepository.GetQuestionsAsync(query.ApplicationId);

        query.Result = mapper.Map<List<QuestionsDto>>(result);
    }
}