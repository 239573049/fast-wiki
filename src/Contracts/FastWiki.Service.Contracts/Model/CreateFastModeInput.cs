namespace FastWiki.Service.Contracts.Model;

public class CreateFastModeInput
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
}