using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Store;

public static class RepositoryExtensions
{
    public static async Task<PagedData<TEntity>> GetPagedDataAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, int offset, int limit, Expression<Func<TEntity, bool>> filter)
        where TEntity : IEntity<TKey>
    {
        var pagedData = await repository.GetPagedAsync(offset, limit, filter);
        var count = await repository.GetCountAsync(filter);
        return new PagedData<TEntity>()
        {
            Data = pagedData,
            Total = count,
        };
    }
}
