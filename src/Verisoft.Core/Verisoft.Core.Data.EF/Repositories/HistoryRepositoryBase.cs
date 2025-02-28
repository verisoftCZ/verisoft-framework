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

public abstract class HistoryRepositoryBase<TEntity, TKey>(
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : EntityFrameworkRepositoryBase<TEntity, TKey>(
            unitOfWork,
            userContext),
        IHistoryRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private HistoryFilters currentHistoryFilters;

    public async Task<IEnumerable<TEntity>> GetPagedHistoryAsync<TSort>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>> filter,
        SortDefinition sort,
        HistoryFilters historyFilters,
        bool isAscendingSortByDefault = true)
        where TSort : BaseSort<TEntity>
    {
        currentHistoryFilters = historyFilters;

        var results = await GetPagedSortAsync<TSort>(
            offset,
            limit,
            filter,
            sort,
            isAscendingSortByDefault);

        currentHistoryFilters = null;

        return results;
    }

    public async Task<int> GetHistoryCountAsync(
        Expression<Func<TEntity, bool>> filter,
        HistoryFilters historyFilters)
    {
        currentHistoryFilters = historyFilters;

        var count = await GetCountAsync(filter);

        currentHistoryFilters = null;

        return count;
    }

    protected override IQueryable<TEntity> FilterData(
        Expression<Func<TEntity, bool>> filter)
    {
        var query = ApplyTemporalFilter(GetDbSet(), currentHistoryFilters);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyTemporalFilter(
    DbSet<TEntity> dbSet,
    HistoryFilters historyFilters)
    {
        if (historyFilters is null)
        {
            return dbSet;
        }

        return historyFilters.QueryType switch
        {
            HistoryQueryType.AsOf when historyFilters.StartUtc.HasValue =>
                dbSet.TemporalAsOf(historyFilters.StartUtc.Value),

            HistoryQueryType.AsOf =>
                throw new ArgumentException("StartUtc must be set for AsOf queries."),

            HistoryQueryType.Between when historyFilters is { StartUtc: not null, EndUtc: not null } =>
                dbSet.TemporalBetween(historyFilters.StartUtc.Value, historyFilters.EndUtc.Value),

            HistoryQueryType.Between =>
                throw new ArgumentException("Both StartUtc and EndUtc must be set for Between queries."),

            HistoryQueryType.ContainedIn when historyFilters is { StartUtc: not null, EndUtc: not null } =>
                dbSet.TemporalContainedIn(historyFilters.StartUtc.Value, historyFilters.EndUtc.Value),

            HistoryQueryType.ContainedIn =>
                throw new ArgumentException("Both StartUtc and EndUtc must be set for ContainedIn queries."),

            _ => dbSet,
        };
    }
}