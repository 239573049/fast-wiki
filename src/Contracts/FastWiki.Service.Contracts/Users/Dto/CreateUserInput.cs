namespace FastWiki.Service.Contracts.Users.Dto;

public class CreateUserInput
{
    /// <summary>
    ///     �˻�
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    ///     �ǳ�
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     ����
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     �ֻ���
    /// </summary>
    public string Phone { get; set; }
}