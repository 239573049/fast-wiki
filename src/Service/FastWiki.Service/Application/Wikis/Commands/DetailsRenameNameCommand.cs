namespace FastWiki.Service.Application.Wikis.Commands;

/// <summary>
///     �޸�֪ʶ����������
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record DetailsRenameNameCommand(long Id, string Name) : Command;