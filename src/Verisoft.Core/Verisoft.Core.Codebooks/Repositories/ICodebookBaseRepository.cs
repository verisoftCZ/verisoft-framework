using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Repositories;

public interface ICodebookBaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
     where TEntity : IBaseEntity<TKey>
{
    async Task<TEntity> IRepository<TEntity, TKey>.GetAsync(TKey id) => await GetAsync(id) ?? throw new InvalidOperationException();

    new Task<TEntity?> GetAsync(TKey id);

    new Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>>? filter,
        SortDefinition sort)
        where TSort : BaseSort<TEntity>, new();

    Task<IEnumerable<TEntity>> GetFilteredSortForExportAsync<TSort>(
        Expression<Func<TEntity, bool>>? filter,
        SortDefinition? sort)
        where TSort : BaseSort<TEntity>, new();
}