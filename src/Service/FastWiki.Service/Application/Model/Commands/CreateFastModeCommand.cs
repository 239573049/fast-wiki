using FastWiki.Service.Contracts.Model;

namespace FastWiki.Service.Application.Model.Commands;

/// <summary>
/// ����ģ��
/// </summary>
/// <param name="Input"></param>
public record CreateFastModeCommand(CreateFastModeInput Input) : Command;