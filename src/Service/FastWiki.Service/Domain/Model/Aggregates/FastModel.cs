namespace FastWiki.Service.Domain.Model.Aggregates;

public sealed class FastModel : FullAggregateRoot<string, Guid?>
{
    public string Name { get; set; }

    /// <summary>
    /// ģ������
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// ģ�ʹ����ַ
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// ģ����Կ
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// AI֧�ֵ�ģ��
    /// </summary>
    public List<string> Models { get; set; } = [];

    /// <summary>
    /// ���ȼ�
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public long? TestTime { get; set; }

    /// <summary>
    /// ���������
    /// </summary>
    public long UsedQuota { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; private set; }

    protected FastModel()
    {
    }

    public FastModel(string name, string type, string url, string apiKey, string description, List<string> models,
        int order)
    {
        Id = Guid.NewGuid().ToString("N");
        Name = name;
        Type = type;
        Url = url;
        ApiKey = apiKey;
        Description = description;
        Models = models;
        Order = order;
        TestTime = null;
        SetEnable(true);
    }

    public void SetEnable(bool enable)
    {
        Enable = enable;
    }
}