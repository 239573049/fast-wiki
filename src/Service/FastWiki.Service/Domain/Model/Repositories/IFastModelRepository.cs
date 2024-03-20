using FastWiki.Service.Domain.Model.Aggregates;

namespace FastWiki.Service.Domain.Model.Repositories;

public interface IFastModelRepository : IRepository<FastModel, string>
{
    /// <summary>
    /// ��ȡģ���б�
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<List<FastModel>> GetModelListAsync(string keyword, int page, int pageSize);
    
    /// <summary>
    /// ��ȡģ������
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    Task<long> GetModelCountAsync(string keyword);   
    
    /// <summary>
    /// �ж�ģ���Ƿ����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> ExistAsync(string name);

    /// <summary>
    /// ɾ��ָ��ģ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(string id);
}