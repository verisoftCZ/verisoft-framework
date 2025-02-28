using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Data.EntityFramework.Repositories;

public abstract class EntityFrameworkRepositoryBase<TEntity, TKey>(
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public IUnitOfWork UnitOfWork { get; } = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        var query = this.GetQueryable();
        return await query.AnyAsync(x => x.Id!.Equals(id));
    }

    public virtual Task<TEntity> GetAsync(TKey id)
    {
        var query = GetQueryable();
        return query.FirstOrDefaultAsync(t => t.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int offset, int limit, Expression<Func<TEntity, bool>> filter)
    {
        var query = FilterData(filter);

        return await query.Skip(offset).Take(limit).ToArrayAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedSortAsync<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<TEntity>
    {
        var query = FilterData(filter);

        var sortField = sort?.Field.ToLower();
        var sortByAsc = sort?.Direction is null or SortDirection.Asc;
        var entitySort = (TSort)Activator.CreateInstance(typeof(TSort), sortField, sortByAsc, isAscendingSortByDefault);
        query = entitySort.ApplySort(query);

        return await query.Skip(offset).Take(limit).ToArrayAsync();
    }

    public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter)
    {
        var query = FilterData(filter);

        return query.CountAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter)
    {
        var query = FilterData(filter);
        return await query.ToArrayAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await FilterData(null).ToArrayAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity is IBaseEntity<TKey> baseEntity)
        {
            baseEntity.CreatedAt = DateTime.UtcNow;
            baseEntity.CreatedBy = userContext.User?.Identity?.Name ?? string.Empty;
        }

        await GetDbSet().AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        if (entity is IBaseEntity<TKey> baseEntity)
        {
            baseEntity.UpdatedAt = DateTime.UtcNow;
            baseEntity.UpdatedBy = userContext.User?.Identity?.Name ?? string.Empty;
        }

        GetDbSet().Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        if (entity is IBaseEntity<TKey> baseEntity)
        {
            baseEntity.IsDeleted = true;
            Update(entity);
        }
        else
        {
            GetDbSet().Remove(entity);
        }
    }

    public virtual void RemoveRange(TEntity[] entities)
    {
        if (entities is IEnumerable<IBaseEntity<TKey>>)
        {
            foreach (var baseEntity in entities)
            {
                Remove(baseEntity);
            }
        }
        else
        {
            GetDbSet().RemoveRange(entities);
        }
    }

    public virtual void UpdateRange(TEntity[] entities)
    {
        if (entities is IEnumerable<IBaseEntity<TKey>>)
        {
            foreach (var baseEntity in entities)
            {
                Update(baseEntity);
            }
        }

        GetDbSet().UpdateRange(entities);
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet();
    }

    protected abstract DbSet<TEntity> GetDbSet();

    protected virtual IQueryable<TEntity> FilterData(Expression<Func<TEntity, bool>> filter)
    {
        IQueryable<TEntity> query = GetQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }
}
