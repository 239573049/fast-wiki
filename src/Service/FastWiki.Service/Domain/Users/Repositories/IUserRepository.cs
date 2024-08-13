namespace FastWiki.Service.Domain.Users.Repositories;

/// <summary>
///     �û��ִ�
/// </summary>
public interface IUserRepository : IRepository<User, Guid>
{
    /// <summary>
    ///     ��ȡ�û��б�
    /// </summary>
    Task<List<User>> GetListAsync(string? keyword, int page, int pageSize);

    /// <summary>
    ///     ��ȡ�û�����
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? keyword);

    /// <summary>
    ///     ɾ���û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    ///     �����û�/�����û�
    /// </summary>
    /// <param name="id"></param>
    /// <param name="disable">�Ƿ����</param>
    /// <returns></returns>
    Task<bool> DisableAsync(Guid id, bool disable);

    /// <summary>
    ///     �޸Ľ�ɫ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task UpdateRoleAsync(Guid id, RoleType role);

    /// <summary>
    ///     ��֤�˻��Ƿ����
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task<bool> IsExistAccountAsync(string account);
}