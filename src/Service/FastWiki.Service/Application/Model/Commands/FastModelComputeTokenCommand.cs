namespace FastWiki.Service.Application.Model.Commands;

/// <summary>
/// ����ģ��
/// </summary>
public record FastModelComputeTokenCommand(string Id,int RequestToken,int CompleteToken) : Command;