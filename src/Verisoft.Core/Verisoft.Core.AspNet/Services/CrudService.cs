using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.AspNet.Services;

public class CrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TSpecification, TSort, TEntity, TKey>(
    ITypeMapper typeMapper,
    IRepository<TEntity, TKey> repository)
    : ReadService<TContract, TListItem, TFilter, TSpecification, TSort, TEntity, TKey>(typeMapper, repository),
    ICrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>, IEntity
    where TSpecification : ISpecification<TEntity>, new()
    where TFilter : new()
    where TSort : BaseSort<TEntity>, new()
{
    private readonly IRepository<TEntity, TKey> repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public virtual async Task<BusinessActionResult<TContract>> AddAsync(TCreateModel createModel)
    {
        await AddPreMapAsync(createModel);
        TEntity entity = await AddMapAsync(createModel);

        var serviceValidationErrors = await GetAddValidationErrorsAsync(createModel);

        if (serviceValidationErrors.HasErrors)
        {
            return serviceValidationErrors;
        }

        await PreAddAsync(entity, createModel);
        await repository.AddAsync(entity);
        await PostAddAsync(entity, createModel);
        await repository.UnitOfWork.CommitAsync();
        await PostAddCommitAsync(entity, createModel);
        return MapToContract(entity);
    }

    public virtual async Task<BusinessActionResult<TContract>> UpdateAsync(TKey id, TEditModel editModel)
    {
        var entity = await repository.GetAsync(id);

        if (entity is null)
        {
            return ErrorFactory.NotFound<TEntity>(nameof(BaseEntity<TKey>.Id), id?.ToString());
        }

        var serviceValidationErrors = await GetUpdateValidationErrorsAsync(id, entity, editModel);

        if (serviceValidationErrors.HasErrors)
        {
            return serviceValidationErrors;
        }

        await PreUpdateAsync(id, entity, editModel);
        entity = await UpdateMapAsync(id, entity, editModel);
        await PostUpdateAsync(id, entity, editModel);
        await repository.UnitOfWork.CommitAsync();

        await PostUpdateCommitAsync(id, entity, editModel);
        return MapToContract(entity);
    }

    public virtual async Task<BusinessActionResult> RemoveAsync(TKey id)
    {
        var entity = await repository.GetAsync(id);

        if (entity is null)
        {
            return ErrorFactory.NotFound<TEntity>(nameof(BaseEntity<TKey>.Id), id?.ToString());
        }

        var serviceValidationErrors = await GetRemoveValidationErrorsAsync(entity);

        if (serviceValidationErrors.HasErrors)
        {
            return serviceValidationErrors;
        }

        await PreRemoveAsync(entity, id);
        repository.Remove(entity);
        await PostRemoveAsync(entity, id);
        await repository.UnitOfWork.CommitAsync();
        await PostRemoveCommitAsync(entity, id);
        return new BusinessActionResult();
    }

    public virtual async Task<BusinessActionResult> AddBulkAsync(TCreateModel[] createModels)
    {
        var entities = new List<TEntity>();
        var errors = new List<BusinessActionError>();
        foreach (var createModel in createModels)
        {
            var entity = await AddMapAsync(createModel);
            var serviceValidationErrors = await GetAddValidationErrorsAsync(createModel);

            if (serviceValidationErrors.HasErrors)
            {
                errors.AddRange(serviceValidationErrors);
            }

            entities.Add(entity);
        }

        if (errors.Any())
        {
            return BusinessActionErrors.FromList(errors);
        }

        await PreAddRangeAsync(entities, createModels);
        await repository.AddRangeAsync(entities);
        await PostAddRangeAsync(entities, createModels);
        await repository.UnitOfWork.CommitAsync();
        await PostAddCommitAsync(entities, createModels);
        return new BusinessActionResult();
    }

    public virtual async Task<BusinessActionResult> RemoveBulkAsync(List<TKey> ids)
    {
        var entities = await repository.GetFilteredAsync(x => ids.Contains(x.Id));
        var enumeratedEntities = entities?.ToArray() ?? [];
        var errors = new List<BusinessActionError>();
        foreach (var entity in enumeratedEntities)
        {
            var serviceValidationErrors = await GetRemoveValidationErrorsAsync(entity);
            if (serviceValidationErrors.HasErrors)
            {
                errors.AddRange(serviceValidationErrors);
            }
        }

        if (errors.Any())
        {
            return BusinessActionErrors.FromList(errors);
        }

        await PreRemoveRangeAsync(enumeratedEntities, ids);
        repository.RemoveRange([.. enumeratedEntities]);
        await PostRemoveRangeAsync(enumeratedEntities, ids);
        await repository.UnitOfWork.CommitAsync();
        await PostRemoveCommitAsync(enumeratedEntities);
        return new BusinessActionResult();
    }

    protected virtual Task<BusinessActionErrors> GetAddValidationErrorsAsync(TCreateModel createModel)
        => Task.FromResult(BusinessActionErrors.None());

    protected virtual Task<BusinessActionErrors> GetUpdateValidationErrorsAsync(TKey id, TEntity entity, TEditModel editModel)
        => Task.FromResult(BusinessActionErrors.None());

    protected virtual Task<BusinessActionErrors> GetRemoveValidationErrorsAsync(TEntity entity)
        => Task.FromResult(BusinessActionErrors.None());

    protected virtual Task AddPreMapAsync(TCreateModel createModel)
        => Task.CompletedTask;

    protected virtual Task PreAddAsync(TEntity entity, TCreateModel createModel)
        => Task.CompletedTask;

    protected virtual Task<TEntity> AddMapAsync(TCreateModel createModel)
    {
        var entity = TypeMapper.Map<TEntity>(createModel);
        return Task.FromResult(entity);
    }

    protected virtual Task PostAddAsync(TEntity entity, TCreateModel createModel)
        => Task.CompletedTask;

    protected virtual Task PostAddCommitAsync(TEntity entity, TCreateModel createModel)
        => Task.CompletedTask;

    protected virtual Task PostAddCommitAsync(List<TEntity> entities, TCreateModel[] createModels)
        => Task.CompletedTask;

    protected virtual Task PreAddRangeAsync(List<TEntity> entities, TCreateModel[] createModels)
        => Task.CompletedTask;

    protected virtual Task PostAddRangeAsync(List<TEntity> entities, TCreateModel[] createModels)
        => Task.CompletedTask;

    protected virtual Task PreRemoveAsync(TEntity entity, TKey id)
        => Task.CompletedTask;

    protected virtual Task PostRemoveAsync(TEntity entity, TKey id)
        => Task.CompletedTask;

    protected virtual Task PostRemoveCommitAsync(IEnumerable<TEntity> entities)
        => Task.CompletedTask;

    protected virtual Task PostRemoveCommitAsync(TEntity entity, TKey id)
        => Task.CompletedTask;

    protected virtual Task PostRemoveRangeAsync(TEntity[] enumeratedEntities, List<TKey> ids)
        => Task.CompletedTask;

    protected virtual Task PreRemoveRangeAsync(TEntity[] enumeratedEntities, List<TKey> ids)
        => Task.CompletedTask;

    protected virtual Task PreUpdateAsync(TKey id, TEntity entity, TEditModel editModel)
        => Task.CompletedTask;

    protected virtual Task<TEntity> UpdateMapAsync(TKey id, TEntity entity, TEditModel editModel)
    {
        TypeMapper.Map(editModel, entity);
        return Task.FromResult(entity);
    }

    protected virtual Task PostUpdateAsync(TKey id, TEntity entity, TEditModel editModel)
        => Task.CompletedTask;

    protected virtual Task PostUpdateCommitAsync(TKey id, TEntity entity, TEditModel editModel)
        => Task.CompletedTask;
}
