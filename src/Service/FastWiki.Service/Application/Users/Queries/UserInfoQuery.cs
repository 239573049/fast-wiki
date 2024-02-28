using FastWiki.Service.Contracts.Users.Dto;

namespace FastWiki.Service.Application.Users.Queries;

/// <summary>
/// ͨ���˺������ȡ�û���Ϣ
/// </summary>
/// <param name="Account"></param>
/// <param name="Pass"></param>
public record UserInfoQuery(string Account,string Pass):Query<UserDto>
{
    public override UserDto Result { get; set; }
}