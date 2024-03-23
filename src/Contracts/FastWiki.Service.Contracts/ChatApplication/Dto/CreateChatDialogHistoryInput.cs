namespace FastWiki.Service.Contracts.ChatApplication.Dto;

public sealed class CreateChatDialogHistoryInput 
{
    public string? Id { get; set; }
    
    /// <summary>
    /// �Ի�id
    /// </summary>
    public string ChatDialogId { get; set; }

    /// <summary>
    /// �Ի�����
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// �Ƿ���
    /// </summary>
    public bool Current { get; set; }

    /// <summary>
    /// �Ի�����
    /// </summary>
    public ChatDialogHistoryType Type { get; set; }

    /// <summary>
    /// ����token
    /// </summary>
    public int ExpendToken { get; set; }
    
    /// <summary>
    /// Դ�ļ�����
    /// </summary>
    public List<SourceFileDto> ReferenceFile { get; set; } = [];
}