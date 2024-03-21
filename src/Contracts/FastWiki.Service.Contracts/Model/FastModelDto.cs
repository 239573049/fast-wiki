namespace FastWiki.Service.Contracts.Model;

public class FastModelDto
{
    public string Id { get; set; }
    
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
    public long TestTime { get; set; }

    /// <summary>
    /// ���������
    /// </summary>
    public long UsedQuota { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; }
}