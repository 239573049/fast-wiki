namespace FastWiki.Service.Application.Users.Commands;

/// <summary>
/// �޸��û���ɫ����
/// </summary>
/// <param name="Id"></param>
/// <param name="Role"></param>
public record UpdateRoleCommand(Guid Id, RoleType Role):Command;