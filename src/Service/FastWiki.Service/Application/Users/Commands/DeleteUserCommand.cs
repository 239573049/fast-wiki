namespace FastWiki.Service.Application.Users.Commands;

/// <summary>
/// ɾ���û�����
/// </summary>
/// <param name="Id"></param>
public record DeleteUserCommand(Guid Id):Command;