using FastWiki.Service.Contracts.Model;

namespace FastWiki.Service.Application.Model.Queries;

/// <summary>
/// ��ȡģ����Ϣ
/// </summary>
/// <param name="Id"></param>
public record ModelInfoQuery(string Id):Query<FastModelDto>
{
    public override FastModelDto Result { get; set; }
}
