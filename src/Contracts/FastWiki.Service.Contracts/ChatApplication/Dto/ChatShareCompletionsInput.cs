namespace FastWiki.Service.Contracts.ChatApplication.Dto;

public class ChatShareCompletionsInput
{
    /// <summary>
    ///     �Ի�Id
    /// </summary>
    public string ChatDialogId { get; set; }

    /// <summary>
    ///     ����Id
    /// </summary>
    public string ChatShareId { get; set; }

    /// <summary>
    ///     �Ի�����
    /// </summary>
    public string Content { get; set; }
}