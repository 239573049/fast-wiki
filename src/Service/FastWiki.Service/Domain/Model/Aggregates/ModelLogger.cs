namespace FastWiki.Service.Domain.Model.Aggregates;

public sealed class ModelLogger : Entity<long>
{
    public DateTime CreationTime { get; protected set; }

    /// <summary>
    /// �󶨵�ģ��ID
    /// </summary>
    public string FastModelId { get; set; }

    /// <summary>
    /// �û�Id
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Ӧ��Id
    /// </summary>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// ����Key
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// ��־����
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// ʹ��ģ��
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// ��ʾʹ��Token����
    /// </summary>
    public int PromptCount { get; set; }

    /// <summary>
    /// ��ȫʹ��Token����
    /// </summary>
    public int ComplementCount { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }

    protected ModelLogger()
    {
    }

    public ModelLogger(string fastModelId, Guid? userId, string? applicationId, string apiKey, string type,
        string model, int promptCount, int complementCount, string description)
    {
        FastModelId = fastModelId;
        UserId = userId;
        ApplicationId = applicationId;
        ApiKey = apiKey;
        Type = type;
        Model = model;
        PromptCount = promptCount;
        ComplementCount = complementCount;
        Description = description;
        CreationTime = DateTime.Now;
    }
}