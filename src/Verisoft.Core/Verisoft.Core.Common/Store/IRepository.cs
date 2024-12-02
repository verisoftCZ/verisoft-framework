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

    Task UpdateAsync(TEntity entity);

    Task UpdateRangeAsync(TEntity[] entities);

    Task RemoveAsync(TEntity entity);

    Task RemoveRangeAsync(TEntity[] entities);

    Task<IEnumerable<EntityAuditEntity>> GetPagedEntityAuditAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<EntityAuditEntity>;

    Task<int> GetEntityAuditCountAsync(Expression<Func<EntityAuditEntity, bool>> filter);

    Task<IEnumerable<EntityAuditDetailEntity>> GetPagedEntityAuditDetailAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditDetailEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<EntityAuditDetailEntity>;

    Task<int> GetEntityAuditDetailCountAsync(Expression<Func<EntityAuditDetailEntity, bool>> filter);
}