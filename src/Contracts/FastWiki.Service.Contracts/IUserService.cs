namespace FastWiki.Service.Contracts;

public interface IUserService
{
    Task<string> GetTokenAsync();

    /// <summary>
    /// �˳���¼
    /// </summary>
    /// <returns></returns>
    Task LogoutAsync();
}