using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IClientService : ICrudService<Client, ClientEditModel, ClientEditModel, ClientListItem, ClientFilter, ClientEntity, int>
{
    Task<BusinessActionResult<Client>> GetClientAsync(int id);

    Task<BusinessActionResult<ClientProjectionExample>> GetOneClientProjectedAsync(int id);

    Task<PagedData<ClientListItem>> GetClientListAsync(FilteredRequest<ClientFilter> request);

    Task<PagedData<ClientProjectionExample>> GetClientsProjectedAsync(FilteredRequest<ClientFilter> request);

    Task<BusinessActionResult<ClientListData>> GetClientHistoryListAsync(FilteredRequest<ClientHistoryFilter> request);

    Task<BusinessActionResult<Client>> AddClientAsync(ClientEditModel clientEditModel);

    Task<BusinessActionResult<Client>> UpdateClientAsync(ClientEditModel clientEditModel, int id);

    Task<BusinessActionResult> RemoveClientAsync(int id);
}