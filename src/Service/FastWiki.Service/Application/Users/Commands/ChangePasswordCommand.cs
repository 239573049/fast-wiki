namespace FastWiki.Service.Application.Users.Commands;

/// <summary>
/// �޸���������
/// </summary>
/// <param name="Id"></param>
/// <param name="Password"></param>
/// <param name="NewPassword"></param>
public record ChangePasswordCommand(Guid Id,string Password,string NewPassword) : Command;