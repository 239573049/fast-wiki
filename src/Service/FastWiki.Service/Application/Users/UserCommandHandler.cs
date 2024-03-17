using System.Text.RegularExpressions;
using FastWiki.Service.Application.Users.Commands;
using FastWiki.Service.Domain.Users.Repositories;

namespace FastWiki.Service.Application.Users;

public sealed class UserCommandHandler(IUserRepository userRepository)
{
    [EventHandler]
    public async Task ChangePasswordAsync(ChangePasswordCommand command)
    {
        var user = await userRepository.FindAsync(command.Id);

        if (user == null)
        {
            throw new UserFriendlyException("�û�������");
        }

        if (!user.CheckCipher(command.Password))
        {
            throw new UserFriendlyException("�������");
        }

        user.SetPassword(command.NewPassword);

        await userRepository.UpdateAsync(user);
    }

    [EventHandler]
    public async Task DeleteUserAsync(DeleteUserCommand command)
    {
        await userRepository.DeleteAsync(command.Id);
    }

    [EventHandler]
    public async Task DisableUserAsync(DisableUserCommand command)
    {
        await userRepository.DisableAsync(command.Id, command.IsDisable);
    }

    [EventHandler]
    public async Task UpdateRoleAsync(UpdateRoleCommand command)
    {
        await userRepository.UpdateRoleAsync(command.Id, command.Role);
    }

    [EventHandler]
    public async Task CreateUserAsync(CreateUserCommand command)
    {
        // У���˺ź����볤��
        if (command.Input.Account.Length < 6 || command.Input.Account.Length > 20)
            throw new UserFriendlyException("�˺ų��ȱ�����6-20֮��");
        
        if (command.Input.Password.Length < 6 || command.Input.Password.Length > 20)
            throw new UserFriendlyException("���볤�ȱ�����6-20֮��");
        
        // У�������ʽ
        if (!Regex.IsMatch(command.Input.Email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
            throw new UserFriendlyException("�����ʽ����");
        
        // TODO: ��֤�˺��Ƿ����
        if(await userRepository.IsExistAccountAsync(command.Input.Account))
            throw new UserFriendlyException("�˺��Ѵ���");
        
        var user = new User(command.Input.Account, command.Input.Name, command.Input.Password,
            "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/Avatar.jpg", command.Input.Email, command.Input.Phone,
            false);
        await userRepository.AddAsync(user);
    }
}