namespace FastWiki.Service.Application.ChatApplications.Queries;

/// <summary>
///     ��ȡ����Ի�������Ϣ
/// </summary>
/// <param name="Id"></param>
public record ChatShareInfoQuery(string Id) : Query<ChatShareDto>
{
    public override ChatShareDto Result { get; set; }
}