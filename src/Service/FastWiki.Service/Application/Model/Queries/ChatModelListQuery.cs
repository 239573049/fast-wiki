using FastWiki.Service.Contracts.Model;

namespace FastWiki.Service.Application.Model.Queries;

/// <summary>
/// ��ȡ�Ի�ģ���б�
/// </summary>
public record ChatModelListQuery():Query<List<GetFastModelDto>>
{
    public override List<GetFastModelDto> Result { get; set; }
}