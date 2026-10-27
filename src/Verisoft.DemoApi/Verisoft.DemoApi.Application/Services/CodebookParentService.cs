using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.CodebookParent;

namespace Verisoft.DemoApi.Application.Services;

public class CodebookParentService(
    ICodebookParentRepository repository,
    IClientRepository clientRepository,
    ITypeMapper mapper) : ICodebookParentService
{
    public async Task<BusinessActionResult<CodebookParentResponse>> GetByIdAsync(int id)
    {
        var chip = await repository.GetAsync(id);
        if (chip is null)
        {
            return ErrorFactory.NotFound<CodebookParentEntity>(nameof(CodebookParentEntity.Id), id);
        }

        return mapper.Map<CodebookParentResponse>(chip);
    }

    public async Task<PagedData<CodebookParentResponse>> GetPagedAsync(FilteredRequest<CodebookParentFilter> request)
    {
        var specification = mapper.Map<CodebookParentSpecification>(request.Filter);
        var filter = specification?.SatisfiedBy();
        var items = await repository
            .GetPagedSortAsync<CodebookParentSort>(
                request.Offset,
                request.Limit,
                filter,
                request.ParsedSorts);

        var mapped = mapper.Map<CodebookParentResponse>(items.Items ?? []);

        return new PagedData<CodebookParentResponse>
        {
            Total = items.Count,
            Data = mapped,
        };
    }

    public async Task<BusinessActionResult<CodebookParentResponse>> CreateAsync(CodebookParentEditModel model)
    {
        if (model.ClientId is not null)
        {
            var clientExists = await clientRepository.ExistsAsync(model.ClientId.Value);
            if (!clientExists)
            {
                return ErrorFactory.NotFound<ClientEntity>($"{nameof(ClientEntity.Id)}", model.ClientId.Value);
            }
        }

        var entity = mapper.Map<CodebookParentEntity>(model);

        await repository.AddAsync(entity);
        repository.UnitOfWork.Commit();

        var updatedEntity = await repository.GetAsync(entity.Id);
        return mapper.Map<CodebookParentResponse>(updatedEntity);
    }

    public async Task<BusinessActionResult<bool>> ExistsForClientAsync(int clientId)
        => await repository.AnyAsync(x => x.ClientId == clientId);
}