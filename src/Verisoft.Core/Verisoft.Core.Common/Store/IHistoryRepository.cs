using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Store
{
    public interface IHistoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetPagedHistoryAsync<TSort>(
            int offset,
            int limit,
            Expression<Func<TEntity, bool>> filter,
            SortDefinition sort,
            HistoryFilters historyFilters,
            bool isAscendingSortByDefault = true)
            where TSort : BaseSort<TEntity>;

        Task<int> GetHistoryCountAsync(
            Expression<Func<TEntity, bool>> filter,
            HistoryFilters historyFilters);
    }
}