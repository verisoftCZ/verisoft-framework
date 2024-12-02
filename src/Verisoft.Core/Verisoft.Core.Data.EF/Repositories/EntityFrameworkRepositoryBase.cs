using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts;
using Verisoft.Core.Data.EntityFramework.Audit;
using Verisoft.Core.Data.EntityFramework.Extensions;

namespace Verisoft.Core.Data.EntityFramework.Repositories;

public abstract class EntityFrameworkRepositoryBase<TEntity, TKey>(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    ILogger<EntityFrameworkRepositoryBase<TEntity, TKey>> logger,
    bool throwIfAuditFails = false,
    IEnumerable<string> ignoredProperties = null) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    private readonly ILogger<EntityFrameworkRepositoryBase<TEntity, TKey>> logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
        var sortByAsc = sort?.Direction == null || sort.Direction == SortDirection.Asc;
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

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity is IBaseEntity<TKey> baseEntity)
        {
            baseEntity.UpdatedAt = DateTime.UtcNow;
            baseEntity.UpdatedBy = userContext.User?.Identity?.Name ?? string.Empty;
        }

        await AuditEntityChangesAsync(entity);

        GetDbSet().Update(entity);
    }

    public virtual async Task RemoveAsync(TEntity entity)
    {
        if (entity is IBaseEntity<TKey> baseEntity)
        {
            baseEntity.IsDeleted = true;
            await UpdateAsync(entity);
        }
        else
        {
            GetDbSet().Remove(entity);
        }
    }

    public virtual async Task RemoveRangeAsync(TEntity[] entities)
    {
        if (entities is IEnumerable<IBaseEntity<TKey>>)
        {
            foreach (var baseEntity in entities)
            {
                await RemoveAsync(baseEntity);
            }
        }
        else
        {
            GetDbSet().RemoveRange(entities);
        }
    }

    public virtual async Task UpdateRangeAsync(TEntity[] entities)
    {
        if (entities is IEnumerable<IBaseEntity<TKey>>)
        {
            foreach (var baseEntity in entities)
            {
                await UpdateAsync(baseEntity);
            }
        }

        GetDbSet().UpdateRange(entities);
    }

    public virtual async Task<IEnumerable<EntityAuditEntity>> GetPagedEntityAuditAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<EntityAuditEntity>
    {
        var dbContext = GetDbContextOrThrow();

        var tableName = dbContext.GetTableName<TEntity>();
        var schema = dbContext.GetTableSchema<TEntity>();

        var query = dbContext.EntityAudit
            .Where(x => x.Table == tableName && x.Schema == schema)
            .Where(filter);

        var sortField = sort?.Field.ToLower();
        var sortByAsc = sort?.Direction == null || sort.Direction == SortDirection.Asc;
        var entitySort = (TSort)Activator.CreateInstance(typeof(TSort), sortField, sortByAsc, isAscendingSortByDefault);

        query = entitySort.ApplySort(query).Include(x => x.EntityAuditDetails).AsNoTracking();

        var result = await query
            .Skip(offset)
            .Take(limit)
            .ToArrayAsync();

        return result;
    }

    public virtual async Task<IEnumerable<EntityAuditDetailEntity>> GetPagedEntityAuditDetailAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditDetailEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
        where TSort : BaseSort<EntityAuditDetailEntity>
    {
        var dbContext = GetDbContextOrThrow();

        var tableName = dbContext.GetTableName<TEntity>();
        var schema = dbContext.GetTableSchema<TEntity>();

        var query = dbContext.EntityAuditDetail
            .Where(x => x.EntityAudit.Table == tableName && x.EntityAudit.Schema == schema)
            .Where(filter);

        var sortField = sort?.Field.ToLower();
        var sortByAsc = sort?.Direction == null || sort.Direction == SortDirection.Asc;
        var entitySort = (TSort)Activator.CreateInstance(typeof(TSort), sortField, sortByAsc, isAscendingSortByDefault);

        query = entitySort.ApplySort(query).Include(x => x.EntityAudit).AsNoTracking();

        var result = await query
            .Skip(offset)
            .Take(limit)
            .ToArrayAsync();

        return result;
    }

    public virtual async Task<int> GetEntityAuditCountAsync(Expression<Func<EntityAuditEntity, bool>> filter)
    {
        var dbContext = GetDbContextOrThrow();

        var tableName = dbContext.GetTableName<TEntity>();
        var schema = dbContext.GetTableSchema<TEntity>();

        return await dbContext.EntityAudit
            .Where(x => x.Table == tableName && x.Schema == schema)
            .Where(filter)
            .AsNoTracking()
            .CountAsync();
    }

    public virtual async Task<int> GetEntityAuditDetailCountAsync(Expression<Func<EntityAuditDetailEntity, bool>> filter)
    {
        var dbContext = GetDbContextOrThrow();

        var tableName = dbContext.GetTableName<TEntity>();
        var schema = dbContext.GetTableSchema<TEntity>();

        return await dbContext.EntityAuditDetail
            .Where(x => x.EntityAudit.Table == tableName && x.EntityAudit.Schema == schema)
            .Where(filter)
            .AsNoTracking()
            .CountAsync();
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet();
    }

    protected abstract DbSet<TEntity> GetDbSet();

    protected IQueryable<TEntity> FilterData(Expression<Func<TEntity, bool>> filter)
    {
        IQueryable<TEntity> query = GetQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    private EntityFrameworkDbContextBase GetDbContextOrThrow()
    {
        if (UnitOfWork is not EntityFrameworkDbContextBase dbContext)
        {
            throw new InvalidCastException("DbContext does not implement EntityFrameworkDbContextBase.");
        }

        return dbContext;
    }

    private async Task AuditEntityChangesAsync(TEntity entity)
    {
        List<EntityAuditDetailEntity> changes = [];

        try
        {
            var entityBeforeUpdate = await GetQueryable().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));
            changes = ClassInstancesPropertiesComparer.GetChangesInCommonProperties(entityBeforeUpdate, entity, ignoredProperties?.ToList());

            if (changes.Count != 0)
            {
                var dbContext = GetDbContextOrThrow();

                var entityAuditEntity = new EntityAuditEntity
                {
                    Table = dbContext.GetTableName<TEntity>(),
                    EntityId = entity.Id.ToString(),
                    Schema = dbContext.GetTableSchema<TEntity>(),
                    ChangedBy = userContext.User?.GetUserName() ?? string.Empty,
                    ChangedAt = DateTime.UtcNow,
                    EntityAuditDetails = [.. changes],
                };

                await dbContext.EntityAudit.AddAsync(entityAuditEntity);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while auditing entity changes for entity {Id}. {changes}", entity.Id, changes);

            if (throwIfAuditFails)
            {
                throw;
            }
        }
    }
}
