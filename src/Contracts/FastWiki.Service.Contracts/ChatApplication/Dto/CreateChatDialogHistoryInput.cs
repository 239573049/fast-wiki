namespace FastWiki.Service.Contracts.ChatApplication.Dto;

public sealed class CreateChatDialogHistoryInput 
{
    /// <summary>
    /// �Ի�id
    /// </summary>
    public string ChatDialogId { get; set; }

    /// <summary>
    /// Ӧ��Id
    /// </summary>
    public string ChatApplicationId { get; set; }

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
}