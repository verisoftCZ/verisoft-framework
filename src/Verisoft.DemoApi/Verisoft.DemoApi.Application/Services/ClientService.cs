using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.ExportStrategies;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Enums;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services;

public class ClientService(
    IClientRepository clientRepository,
    ExportStrategyResolver<Client> exportStrategyResolver,
    IUserContext httpUserContext,
    ILogger<ClientService> logger,
    ITypeMapper typeMapper)
: IClientService
{
    private const int MaxNumberOfRecords = 10_000;

    public async Task<BusinessActionResult<Client>> AddClientAsync(ClientEditModel clientEditModel)
    {
        if (clientEditModel is null)
        {
            return ErrorFactory.NullInput(nameof(clientEditModel));
        }

        var clientEntity = typeMapper.Map<ClientEntity>(clientEditModel);
        if (clientEditModel.ParentClientId.HasValue)
        {
            var clientParentEntityExists = await clientRepository.ExistsAsync(clientEditModel.ParentClientId.Value);
            if (!clientParentEntityExists)
            {
                return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), clientEditModel.ParentClientId.Value);
            }
        }

        await clientRepository.AddAsync(clientEntity);

        clientRepository.UnitOfWork.Commit();

        return typeMapper.Map<Client>(clientEntity);
    }

    public async Task<BusinessActionResult<Client>> GetClientAsync(int id)
    {
        var clientEntity = await clientRepository.GetAsync(id);
        if (clientEntity is null)
        {
            return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), id);
        }

        return typeMapper.Map<Client>(clientEntity);
    }

    public async Task<ClientListData> GetClientListAsync(FilteredRequest<ClientFilter> request)
    {
        var clientSpecification = typeMapper.Map<ClientSpecification>(request.Filter);
        var filter = clientSpecification?.SatisfiedBy();
        var totalItems = await clientRepository.GetCountAsync(filter);
        var items = await clientRepository.GetPagedSortAsync<ClientSort>(
            request.Offset,
            request.Limit,
            filter,
            request.Sort);
        var mappedClientData = typeMapper.Map<ClientListItem>(items);

        return new ClientListData
        {
            Total = totalItems,
            Data = mappedClientData,
        };
    }

    public async Task<BusinessActionResult<ExportResult>> GetClientsForExportAsync(FilteredRequest<ClientFilter> request, ExportType exportType)
    {
        var clientSpecification = typeMapper.Map<ClientSpecification>(request.Filter);
        var filter = clientSpecification?.SatisfiedBy();
        var items = await clientRepository.GetPagedSortAsync<ClientSort>(0, GetLimit(), filter, request.Sort);

        var clients = typeMapper.Map<Client>(items).ToList();

        try
        {
            var strategy = exportStrategyResolver.Resolve(exportType);
            var result = await strategy.ExportAsync(clients);
            return new BusinessActionResult<ExportResult>(result);
        }
        catch (NotSupportedException)
        {
            return ActionResultFactory.UnsupportedMediaType(exportType.ToString());
        }
    }

    public async Task<BusinessActionResult<Client>> RemoveClientAsync(int id)
    {
        var clientEntity = await clientRepository.GetAsync(id);

        if (clientEntity is null)
        {
            return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), id);
        }

        await clientRepository.RemoveAsync(clientEntity);
        clientRepository.UnitOfWork.Commit();

        return typeMapper.Map<Client>(clientEntity);
    }

    public async Task<BusinessActionResult<Client>> UpdateClientAsync(ClientEditModel clientEditModel, int id)
    {
        if (clientEditModel is null)
        {
            return ErrorFactory.NullInput(nameof(clientEditModel));
        }

        var clienEntity = await clientRepository.GetAsync(id);
        if (clientEditModel.ParentClientId.HasValue)
        {
            var clientParentEntityExists = await clientRepository.ExistsAsync(clientEditModel.ParentClientId.Value);
            if (!clientParentEntityExists)
            {
                return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), clientEditModel.ParentClientId.Value);
            }
        }

        if (clienEntity is null)
        {
            return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), id);
        }

        clienEntity = typeMapper.Map(clientEditModel, clienEntity);
        await clientRepository.UpdateAsync(clienEntity);
        clientRepository.UnitOfWork.Commit();

        return typeMapper.Map<Client>(clienEntity);
    }

    private int GetLimit() => httpUserContext.User.IsInRole(DemoApiPermissions.ExportAllClients.ToString())
        ? int.MaxValue
        : MaxNumberOfRecords;
}