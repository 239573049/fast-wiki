using FastWiki.Service.Contracts.Users.Dto;

namespace FastWiki.Service.Application.Users.Commands;

/// <summary>
/// �����û�����
/// </summary>
/// <param name="Input"></param>
public record CreateUserCommand(CreateUserInput Input) : Command;