using FastWiki.Service.Contracts.Model;

namespace FastWiki.Service.Application.Model.Commands;

/// <summary>
/// �༭ģ��
/// </summary>
/// <param name="Dto"></param>
public record UpdateFastModelCommand(FastModelDto Dto):Command;