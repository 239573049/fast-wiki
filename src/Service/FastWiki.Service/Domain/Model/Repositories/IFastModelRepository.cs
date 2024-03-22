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
    
    /// <summary>
    /// �޸�ģ��
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task UpdateAsync(FastModel model);

    /// <summary>
    /// ���û�����ģ��
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enable"></param>
    /// <returns></returns>
    Task EnableAsync(string id, bool enable);
    
    /// <summary>
    /// ģ�ͼ���token
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestToken"></param>
    /// <param name="completeToken"></param>
    /// <returns></returns>
    Task FastModelComputeTokenAsync(string id,int requestToken,int completeToken);
}