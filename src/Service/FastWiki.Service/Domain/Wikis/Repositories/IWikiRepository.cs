﻿namespace FastWiki.Service.Domain.Wikis.Repositories;

/// <summary>
///     知识库仓储
/// </summary>
public interface IWikiRepository : IRepository<Wiki, long>
{
    /// <summary>
    ///     获取知识库列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<List<Wiki>> GetListAsync(Guid userId, string? keyword, int page, int pageSize);

    /// <summary>
    ///     编辑知识库
    /// </summary>
    /// <param name="wiki"></param>
    /// <returns></returns>
    Task UpdateAsync(Wiki wiki);

    /// <summary>
    ///     获取知识库数量
    /// </summary>
    /// <param name="queryUserId"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(Guid queryUserId, string? keyword);

    /// <summary>
    ///     获取知识库详情列表
    /// </summary>
    /// <param name="wikiId"></param>
    /// <param name="queryState"></param>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<List<WikiDetail>> GetDetailsListAsync(long wikiId, WikiQuantizationState? queryState, string? keyword,
        int page,
        int pageSize);

    /// <summary>
    ///     获取知识库详情数量
    /// </summary>
    /// <param name="wikiId"></param>
    /// <param name="queryState"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    Task<long> GetDetailsCountAsync(long wikiId, WikiQuantizationState? queryState, string? keyword);

    /// <summary>
    ///     添加知识库详情
    /// </summary>
    /// <param name="wikiDetail"></param>
    /// <returns></returns>
    Task<WikiDetail> AddDetailsAsync(WikiDetail wikiDetail);

    /// <summary>
    ///     删除知识库详情
    /// </summary>
    /// <param name="wikiDetailId"></param>
    /// <returns></returns>
    Task RemoveDetailsAsync(long wikiDetailId);

    /// <summary>
    ///     获取知识库详情信息
    /// </summary>
    /// <param name="wikiDetailId"></param>
    /// <returns></returns>
    Task<WikiDetail> GetDetailsAsync(long wikiDetailId);

    /// <summary>
    ///     批量删除详情
    /// </summary>
    /// <param name="wikiDetailIds"></param>
    /// <returns></returns>
    Task RemoveDetailsAsync(List<long> wikiDetailIds);

    /// <summary>
    ///     修改详情状态
    /// </summary>
    /// <param name="wikiDetailId"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    Task UpdateDetailsState(long wikiDetailId, WikiQuantizationState state);

    /// <summary>
    ///     获取失败的详情量化数据
    /// </summary>
    /// <returns></returns>
    Task<List<WikiDetail>> GetFailedDetailsAsync();

    /// <summary>
    ///     删除知识库详情指定的向量数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveDetailsVectorAsync(string index, string id);

    Task DetailsRenameNameAsync(long id, string name);

    /// <summary>
    /// 通过详情id查找知识库
    /// </summary>
    /// <param name="wikiDetailId"></param>
    /// <returns></returns>
    Task<Wiki> WikiDetailGetWikiAsync(long wikiDetailId);

    /// <summary>
    /// 创建一个量化列表任务
    /// </summary>
    /// <param name="wikiId"></param>
    /// <param name="wikiDetailId"></param>
    /// <param name="remark"></param>
    public Task<long> CreateQuantizationListAsync(long wikiId, long wikiDetailId, string remark);

    /// <summary>
    /// 完成量化任务
    /// </summary>
    public Task CompleteQuantizationListAsync(long id, string remark, QuantizedListState state);

    /// <summary>
    /// 获取指定知识库的量化任务列表
    /// </summary>
    public Task<List<QuantizedList>> GetQuantizedListAsync(long wikiId, QuantizedListState? state, int page,
        int pageSize);
    
    /// <summary>
    /// 获取指定知识库的量化任务列表数量
    /// </summary>
    public Task<long> GetQuantizedListCountAsync(long wikiId, QuantizedListState? state);
    
    /// <summary>
    /// 通过wiki详情ids获取wiki详情
    /// </summary>
    public Task<List<WikiDetail>> GetDetailsByIdsAsync(List<long> wikiDetailIds);
}