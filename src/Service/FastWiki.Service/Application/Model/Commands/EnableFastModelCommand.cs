namespace FastWiki.Service.Application.Model.Commands;

/// <summary>
/// ����|����ģ��
/// </summary>
/// <param name="Id"></param>
/// <param name="Enable"></param>
public record EnableFastModelCommand(string Id, bool Enable) : Command;