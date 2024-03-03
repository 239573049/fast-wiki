using System.Threading.Channels;

namespace FastWiki.Service.Backgrounds;

public sealed class QuantizeBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// ��ǰ��������
    /// </summary>
    private static int CurrentTask = 0;

    /// <summary>
    /// �����������
    /// </summary>
    public static int MaxTask = 3;

    private static readonly Channel<WikiDetail> WikiDetails = Channel.CreateBounded<WikiDetail>(
        new BoundedChannelOptions(1000)
        {
            SingleReader = true,
            SingleWriter = false
        });

    /// <summary>
    /// ���캯��
    /// </summary>
    public QuantizeBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// ��̨����
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ��ȡ���������Ƿ����������������
        var QUANTIZE_MAX_TASK = Environment.GetEnvironmentVariable("QUANTIZE_MAX_TASK");
        if (!string.IsNullOrEmpty(QUANTIZE_MAX_TASK))
        {
            int.TryParse(QUANTIZE_MAX_TASK, out MaxTask);
        }

        // TODO: ��������ʱ����ʧ�ܵ�wikiDetail
        await LoadingWikiDetailAsync();
        await Task.Factory.StartNew(WikiDetailHandlerAsync, stoppingToken);
    }

    /// <summary>
    /// ����wikiDetail������
    /// </summary>
    /// <param name="wikiDetail"></param>
    public static async Task AddWikiDetailAsync(WikiDetail wikiDetail)
    {
        await WikiDetails.Writer.WriteAsync(wikiDetail);
    }

    private async Task WikiDetailHandlerAsync()
    {
        // wikiDetailsѭ������
        while (await WikiDetails.Reader.WaitToReadAsync())
        {
            var wikiDetail = await WikiDetails.Reader.ReadAsync();

            if (wikiDetail != null)
            {
                // ��������С������������� ��ִ������
                for (var i = 0; i < 200; i++)
                {
                    if (CurrentTask < MaxTask)
                    {
                        _ = HandlerAsync(wikiDetail);
                        break;
                    }

                    // �ȴ� 1s
                    await Task.Delay(500);
                }
            }
        }
    }

    /// <summary>
    /// ����wikiDetail����������
    /// </summary>
    /// <param name="state"></param>
    private async ValueTask HandlerAsync(object state)
    {
        Interlocked.Increment(ref CurrentTask);
        if (state is WikiDetail wikiDetail)
        {
            using var asyncServiceScope = _serviceProvider.CreateScope();

            var fileStorageRepository = asyncServiceScope.ServiceProvider.GetRequiredService<IFileStorageRepository>();
            var wikiRepository = asyncServiceScope.ServiceProvider.GetRequiredService<IWikiRepository>();
            var serverless = asyncServiceScope.ServiceProvider.GetRequiredService<MemoryServerless>();

            try
            {
                Console.WriteLine($"��ʼ�����ĵ���{wikiDetail.FileName} {wikiDetail.Path} {wikiDetail.FileId}");

                string result = string.Empty;
                if (wikiDetail.Type == "file")
                {
                    // ����wikiDetail
                    var fileInfoQuery = await fileStorageRepository.FindAsync(x => x.Id == wikiDetail.FileId);

                    result = await serverless.ImportDocumentAsync(fileInfoQuery.FullName,
                        wikiDetail.Id.ToString(),
                        tags: new TagCollection()
                        {
                            {
                                "wikiId", wikiDetail.WikiId.ToString()
                            },
                            {
                                "fileId", wikiDetail.FileId.ToString()
                            },
                            {
                                "wikiDetailId", wikiDetail.Id.ToString()
                            }
                        }, "wiki");
                }
                else if (wikiDetail.Type == "web")
                {
                    result = await serverless.ImportWebPageAsync(wikiDetail.Path,
                        wikiDetail.Id.ToString(),
                        tags: new TagCollection()
                        {
                            {
                                "wikiId", wikiDetail.WikiId.ToString()
                            },
                            {
                                "fileId", wikiDetail.FileId.ToString()
                            },
                            {
                                "wikiDetailId", wikiDetail.Id.ToString()
                            }
                        }, "wiki");
                }

                await wikiRepository.UpdateDetailsState(wikiDetail.Id, WikiQuantizationState.Accomplish);
                Console.WriteLine($"�����ĵ���ɣ�{wikiDetail.FileName} {wikiDetail.Path} {wikiDetail.FileId} {result}");
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"�����ĵ�ʧ�ܣ�{wikiDetail.FileName} {wikiDetail.Path} {wikiDetail.FileId} {Environment.NewLine + e.Message}");
                if (wikiDetail.State != WikiQuantizationState.Fail)
                {
                    await wikiRepository.UpdateDetailsState(wikiDetail.Id, WikiQuantizationState.Fail);
                }

                // ���¼������
                await AddWikiDetailAsync(wikiDetail);
            }
        }

        Interlocked.Decrement(ref CurrentTask);
    }

    private async Task LoadingWikiDetailAsync()
    {
        using var asyncServiceScope = _serviceProvider.CreateScope();

        var wikiRepository = asyncServiceScope.ServiceProvider.GetRequiredService<IWikiRepository>();

        // ��ȡʧ�ܵ�wikiDetail
        var wikiDetails = await wikiRepository.GetFailedDetailsAsync();

        foreach (var wikiDetail in wikiDetails)
        {
            await AddWikiDetailAsync(wikiDetail);
        }
    }
}