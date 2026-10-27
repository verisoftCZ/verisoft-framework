using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Store;

public interface IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
{
    IUnitOfWork UnitOfWork { get; }

    Task<bool> ExistsAsync(TKey id);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

    Task<TEntity?> GetAsync(TKey id);

    Task<TProjection?> GetProjectedAsync<TProjection>(TKey id, Expression<Func<TEntity, TProjection>> selector);

    Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(int offset, int limit, Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts)
        where TSort : BaseSort<TEntity>, new();

    Task<(IReadOnlyList<TProjection> Items, int Count)> GetPagedSortProjectedAsync<TSort, TProjection>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>>? filter,
        IEnumerable<SortDefinition>? sorts,
        Expression<Func<TEntity, TProjection>> selector)
        where TSort : BaseSort<TEntity>, new();

    Task<IEnumerable<TEntity>> GetFilteredSortForExportAsync<TSort>(Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts)
        where TSort : BaseSort<TEntity>, new();

    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter);

    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>>? filter);

    Task<IEnumerable<TEntity>> GetAllAsync();

    void MarkAsModified(TEntity entity);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}