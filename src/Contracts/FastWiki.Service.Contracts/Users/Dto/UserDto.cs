namespace FastWiki.Service.Contracts.Users.Dto;

public sealed class UserDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// �˻�
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// ͷ��
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// �Ƿ����
    /// </summary>
    public bool IsDisable { get; set; }

    public RoleType Role { get; set; }

    public string RoleName
    {
        get
        {
            switch (Role)
            {
                case RoleType.Admin:
                    return "����Ա";
                case RoleType.User:
                    return "�û�";
                case RoleType.Guest:
                    return "�ο�";
            }

            return "δ֪";
        }
    }
}