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

    Task<TEntity> GetAsync(TKey id);

    Task<IEnumerable<TEntity>> GetPagedAsync(int offset, int limit, Expression<Func<TEntity, bool>> filter);

    Task<IEnumerable<TEntity>> GetPagedSortAsync<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<TEntity>;

    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter);

    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter);

    Task<IEnumerable<TEntity>> GetAll();

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void UpdateRange(TEntity[] entities);

    void Remove(TEntity entity);

    void RemoveRange(TEntity[] entities);
}