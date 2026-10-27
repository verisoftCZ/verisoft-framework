using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts;
using Verisoft.Core.Data.EntityFramework.Options;

namespace Verisoft.Core.Data.EntityFramework.Repositories;

public abstract class EntityFrameworkRepositoryBase<TEntity, TKey>(
    IUnitOfWork unitOfWork) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await GetQueryable().AnyAsync(x => x.Id!.Equals(id));
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        => await GetDbSet().AnyAsync(filter);

    public virtual async Task<TEntity?> GetAsync(TKey id)
    {
        return await GetQueryableForDetail().SingleOrDefaultAsync(t => t.Id!.Equals(id));
    }

    public virtual async Task<TProjection?> GetProjectedAsync<TProjection>(TKey id, Expression<Func<TEntity, TProjection>> selector) => await GetQueryableForDetail()
        .Where(t => t.Id!.Equals(id))
        .Select(selector)
        .SingleOrDefaultAsync();

    public virtual async Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(int offset, int limit, Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts)
        where TSort : BaseSort<TEntity>, new()
    {
        var query = FilterData(filter);
        return await GetPagedSortAsync<TSort>(query, offset, limit, sorts, QueryOptions.Default);
    }

    public virtual async Task<(IReadOnlyList<TProjection> Items, int Count)> GetPagedSortProjectedAsync<TSort, TProjection>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>>? filter,
        IEnumerable<SortDefinition>? sorts,
        Expression<Func<TEntity, TProjection>> selector)
        where TSort : BaseSort<TEntity>, new()
    {
        var query = FilterData(filter);

        return await GetPagedSortProjectedAsync<TSort, TProjection>(
            query,
            offset,
            limit,
            sorts,
            selector);
    }

    public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter)
    {
        var query = FilterData(filter);
        return await query.CountAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>>? filter)
    {
        var query = FilterData(filter);
        return await query.ToArrayAsync();
    }

    public async Task<IEnumerable<TEntity>> GetFilteredSortForExportAsync<TSort>(
        Expression<Func<TEntity, bool>>? filter,
        IEnumerable<SortDefinition>? sorts)
        where TSort : BaseSort<TEntity>, new()
    {
        var query = GetQueryableForExport();
        return await GetFilteredSortForExportAsync<TSort>(query, filter, sorts, QueryOptions.Default);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await GetQueryable().ToArrayAsync();
    }

    public virtual void MarkAsModified(TEntity entity)
    {
        GetDbSet().Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await GetDbSet().AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {

        GetDbSet().Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        GetDbSet().Remove(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await GetDbSet().AddRangeAsync(entities);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        GetDbSet().RemoveRange(entities);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        GetDbSet().UpdateRange(entities);
    }

    protected virtual async Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(IQueryable<TEntity> query, int offset, int limit, Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts, QueryOptions? queryOptions = null)
        where TSort : BaseSort<TEntity>, new()
    {
        query = FilterData(filter, query);
        return await GetPagedSortAsync<TSort>(query, offset, limit, sorts, queryOptions);
    }

    protected virtual async Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(IQueryable<TEntity> query, int offset, int limit, IEnumerable<SortDefinition>? sorts, QueryOptions? queryOptions = null)
        where TSort : BaseSort<TEntity>, new()
    {
        queryOptions = queryOptions ?? QueryOptions.Default;
        var count = await query.CountAsync();
        var pagedQuery = ApplySort<TSort>(query, sorts)
            .Skip(offset)
            .Take(limit);
        if (queryOptions.AsNoTracking)
        {
            pagedQuery = pagedQuery.AsNoTracking();
        }

        if (queryOptions.AsSplitQuery)
        {
            pagedQuery = pagedQuery.AsSplitQuery();
        }

        var data = await pagedQuery.ToListAsync();
        return (data, count);
    }

    protected virtual async Task<(IReadOnlyList<TProjection> Items, int Count)> GetPagedSortProjectedAsync<TSort, TProjection>(
        IQueryable<TEntity> query,
        int offset,
        int limit,
        IEnumerable<SortDefinition>? sorts,
        Expression<Func<TEntity, TProjection>> selector)
        where TSort : BaseSort<TEntity>, new()
    {
        var count = await query.CountAsync();

        var sortedQuery = ApplySort<TSort>(
            query,
            sorts);

        var pagedQuery = sortedQuery
            .Skip(offset)
            .Take(limit)
            .Select(selector);

        var data = await pagedQuery.ToListAsync();

        return (data, count);
    }

    protected virtual async Task<IEnumerable<TEntity>> GetFilteredSortForExportAsync<TSort>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>>? filter,
        IEnumerable<SortDefinition>? sorts,
        QueryOptions? queryOptions = null)
        where TSort : BaseSort<TEntity>, new()
    {
        queryOptions = queryOptions ?? QueryOptions.Default;
        query = FilterData(filter, query);
        if (queryOptions.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (queryOptions.AsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return await ApplySort<TSort>(query, sorts).ToArrayAsync();
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet();
    }

    protected virtual IQueryable<TEntity> GetQueryableForDetail()
    {
        return GetDbSet();
    }

    protected virtual IQueryable<TEntity> GetQueryableForExport()
    {
        return GetDbSet();
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
        return UnitOfWork.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> FilterData(Expression<Func<TEntity, bool>>? filter, IQueryable<TEntity>? query = null)
    {
        if (query == null)
        {
            query = GetQueryable();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    protected virtual bool IsAscending(SortDefinition? sort) =>
        sort?.Direction == null || sort.Direction == SortDirection.Asc;

    protected virtual IQueryable<TEntity> ApplySort<TSort>(
        IQueryable<TEntity> query,
        IEnumerable<SortDefinition>? requestedSorts)
        where TSort : BaseSort<TEntity>, new()
    {
        var sortProvider = new TSort();

        var effectiveSorts = BuildEffectiveSorts(
            sortProvider,
            requestedSorts);

        return ApplySorts(query, effectiveSorts);
    }

    private static IEnumerable<SortClause<TEntity>> BuildEffectiveSorts<TSort>(
        TSort provider,
        IEnumerable<SortDefinition>? requestedSorts)
        where TSort : BaseSort<TEntity>
    {
        var resolved = requestedSorts?
            .Where(x => !string.IsNullOrWhiteSpace(x.Field))
            .Select(x => new SortClause<TEntity>(
                provider.GetSortingExpression(GetSortPropertyName(x.Field!)),
                x.Direction))
            .ToList();

        return resolved is { Count: > 0 }
            ? resolved.Concat(provider.DefaultSorts)
            : provider.DefaultSorts;
    }

    private static string GetSortPropertyName(string propertyName)
    {
        var codebookPropertyInfo = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (codebookPropertyInfo is not null && codebookPropertyInfo.PropertyType.IsCodebookRecord())
        {
            return propertyName + CodebookRecordEntityConstants.CodebookRecordForeignKeyPropertySuffix;
        }

        return propertyName;
    }

    private static IQueryable<TEntity> ApplySorts(
        IQueryable<TEntity> query,
        IEnumerable<SortClause<TEntity>> sorts)
    {
        using var enumerator = sorts.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return query;
        }

        var first = enumerator.Current;

        var ordered = first.Direction == SortDirection.Asc
            ? query.OrderBy(first.Expression)
            : query.OrderByDescending(first.Expression);

        while (enumerator.MoveNext())
        {
            var sort = enumerator.Current;

            ordered = sort.Direction == SortDirection.Asc
                ? ordered.ThenBy(sort.Expression)
                : ordered.ThenByDescending(sort.Expression);
        }

        return ordered;
    }
}