using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.AspNet.Services.Interfaces;

public interface IReadService<TContract, TListItem, TFilter, TEntity, TKey>
{
    Task<bool> ExistsAsync(TKey id);

    Task<BusinessActionResult<TContract>> GetAsync(TKey id);

    Task<BusinessActionResult<TProjection>> GetProjectedAsync<TProjection>(TKey id, Expression<Func<TEntity, TProjection>> selector);

    Task<PagedData<TListItem>> GetListAsync(FilteredRequest<TFilter> request);

    Task<PagedData<TProjection>> GetProjectedListAsync<TProjection>(
        FilteredRequest<TFilter> request,
        Expression<Func<TEntity, TProjection>> selector);
}