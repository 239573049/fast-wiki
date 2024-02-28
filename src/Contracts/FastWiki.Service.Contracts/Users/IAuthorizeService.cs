using FastWiki.Service.Contracts.Users.Dto;

namespace FastWiki.Service.Contracts.Users;

public interface IAuthorizeService
{
    /// <summary>
    /// ��¼��ȡtoken
    /// </summary>
    /// <param name="account"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    Task<AuthorizeDto> TokenAsync(string account,string pass);
}