using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Store;

public interface IHistoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    Task<(IEnumerable<TEntity> Items, int Count)> GetPagedHistoryAsync<TSort>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<SortDefinition>? sorts,
        HistoryFilters historyFilters)
        where TSort : BaseSort<TEntity>, new();

    Task<int> GetHistoryCountAsync(
        Expression<Func<TEntity, bool>> filter,
        HistoryFilters historyFilters);
}