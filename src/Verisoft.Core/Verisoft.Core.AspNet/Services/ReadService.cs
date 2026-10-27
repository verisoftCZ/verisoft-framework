using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.AspNet.Services;

public abstract class ReadService<TContract, TListItem, TFilter, TSpecification, TSort, TEntity, TKey>(
    ITypeMapper typeMapper,
    IRepository<TEntity, TKey> repository)
    : IReadService<TContract, TListItem, TFilter, TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>, IEntity
    where TSpecification : ISpecification<TEntity>, new()
    where TFilter : new()
    where TSort : BaseSort<TEntity>, new()
{
    private readonly IRepository<TEntity, TKey> repository = repository ?? throw new ArgumentNullException(nameof(repository));

    protected ITypeMapper TypeMapper { get; } = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await repository.ExistsAsync(id);
    }

    public virtual async Task<BusinessActionResult<TContract>> GetAsync(TKey id)
    {
        var entity = await repository.GetAsync(id);
        if (entity is null)
        {
            return ErrorFactory.NotFound<TEntity>(nameof(BaseEntity<TKey>.Id), id?.ToString());
        }

        return MapToContract(entity);
    }

    public virtual async Task<BusinessActionResult<TProjection>> GetProjectedAsync<TProjection>(TKey id, Expression<Func<TEntity, TProjection>> selector)
    {
        var entityProjection = await repository.GetProjectedAsync(id, selector);
        if (entityProjection is null)
        {
            return ErrorFactory.NotFound<TEntity>(nameof(BaseEntity<TKey>.Id), id?.ToString());
        }

        return entityProjection;
    }

    public virtual async Task<PagedData<TListItem>> GetListAsync(FilteredRequest<TFilter> request)
    {
        request.Filter ??= new();

        var specification = TypeMapper.Map<TSpecification>(request.Filter) ?? new();
        await PreGetListAsync(specification, request);
        var filter = specification.SatisfiedBy();
        var (entities, count) = await repository.GetPagedSortAsync<TSort>(request.Offset, request.Limit, filter, request.ParsedSorts);
        var mapped = MapToListItems(entities);
        await PostMapListAsync(request, mapped, entities);
        return new PagedData<TListItem>
        {
            Data = mapped,
            Total = count,
        };
    }

    public virtual async Task<PagedData<TProjection>> GetProjectedListAsync<TProjection>(
        FilteredRequest<TFilter> request,
        Expression<Func<TEntity, TProjection>> selector)
    {
        request.Filter ??= new();

        var specification = TypeMapper.Map<TSpecification>(request.Filter) ?? new();

        await PreGetListAsync(specification, request);

        var filter = specification.SatisfiedBy();

        var (items, count) = await repository.GetPagedSortProjectedAsync<TSort, TProjection>(
            request.Offset,
            request.Limit,
            filter,
            request.ParsedSorts,
            selector);

        return new PagedData<TProjection>
        {
            Data = items,
            Total = count,
        };
    }

    protected virtual TContract MapToContract(TEntity entity)
        => TypeMapper.Map<TContract>(entity);

    protected virtual IEnumerable<TListItem> MapToListItems(IEnumerable<TEntity> entities)
        => TypeMapper.Map<TListItem>(entities);

    protected virtual Task PostMapListAsync(FilteredRequest<TFilter> request, IEnumerable<TListItem> mapped, IEnumerable<TEntity> entities)
        => Task.CompletedTask;

    protected virtual Task PreGetListAsync(TSpecification specification, FilteredRequest<TFilter> request)
        => Task.CompletedTask;
}