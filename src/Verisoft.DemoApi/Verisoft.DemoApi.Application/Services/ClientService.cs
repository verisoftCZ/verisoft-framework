using Verisoft.Core.AspNet.Services;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services;

public class ClientService(
    IClientRepository clientRepository,
    ITypeMapper typeMapper)
    : CrudService<Client, ClientEditModel, ClientEditModel, ClientListItem, ClientFilter, ClientSpecification, ClientSort, ClientEntity, int>(typeMapper, clientRepository), IClientService
{
    public async Task<BusinessActionResult<Client>> AddClientAsync(ClientEditModel clientEditModel) => await AddAsync(clientEditModel);

    public async Task<BusinessActionResult<Client>> GetClientAsync(int id) => await GetAsync(id);

    public async Task<PagedData<ClientListItem>> GetClientListAsync(FilteredRequest<ClientFilter> request) => await GetListAsync(request);

    public async Task<BusinessActionResult<ClientListData>> GetClientHistoryListAsync(FilteredRequest<ClientHistoryFilter> request)
    {
        var clientSpecification = TypeMapper.Map<ClientSpecification>(request.Filter!);
        var filter = clientSpecification.SatisfiedBy();
        var historyFilters = new HistoryFilters(request.Filter!.HistoryQueryType, request.Filter?.PeriodStart, request.Filter?.PeriodEnd);
        try
        {
            var items = await clientRepository.GetPagedHistoryAsync<ClientSort>(
                request.Offset,
                request.Limit,
                filter,
                request.ParsedSorts,
                historyFilters);
            var mappedClientData = TypeMapper.Map<ClientListItem>(items.Items);

            return new ClientListData
            {
                Total = items.Count,
                Data = mappedClientData,
            };
        }
        catch (ArgumentException ex)
        {
            return ErrorFactory.BadRequest(ex.Message);
        }
    }

    public async Task<BusinessActionResult> RemoveClientAsync(int id) => await RemoveAsync(id);

    public async Task<BusinessActionResult<Client>> UpdateClientAsync(ClientEditModel clientEditModel, int id) => await UpdateAsync(id, clientEditModel);

    public Task<PagedData<ClientProjectionExample>> GetClientsProjectedAsync(FilteredRequest<ClientFilter> request) => GetProjectedListAsync(
            request,
            x => new ClientProjectionExample
            {
                ClientName = x.Name!,
                DocumentNames = x.Documents.Select(d => d.Name).ToList(),
            });

    public async Task<BusinessActionResult<ClientProjectionExample>> GetOneClientProjectedAsync(int id)
    {
        return await GetProjectedAsync(id, x => new ClientProjectionExample
        {
            ClientName = x.Name!,
            DocumentNames = x.Documents.Select(d => d.Name).ToList(),
        });
    }

    protected override async Task<ClientEntity> AddMapAsync(ClientEditModel? createModel)
    {
        // Example of custom mapping logic during creation, if needed. Otherwise, the base mapping will be used.
        await Task.Yield(); // Simulate async work before mapping if necessary
        var mapped = await base.AddMapAsync(createModel);
        await Task.Yield(); // Simulate async after mapping if necessary
        return mapped;
    }

    // Example of custom validation logic during creation, if needed. Otherwise, the base validation will be used.
    protected override async Task<BusinessActionErrors> GetAddValidationErrorsAsync(ClientEditModel createModel)
    {
        if (string.IsNullOrEmpty(createModel.Name) || createModel.Name.Length < 10)
        {
            return ErrorFactory.UnprocessableEntity("Clients must have really long names of at least 10 characters", new { createModel.Name });
        }

        return await base.GetAddValidationErrorsAsync(createModel);
    }

}