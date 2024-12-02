using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IClientService
{
    Task<BusinessActionResult<Client>> GetClientAsync(int id);

    Task<ClientListData> GetClientListAsync(FilteredRequest<ClientFilter> request);

    Task<BusinessActionResult<ExportResult>> GetClientsForExportAsync(FilteredRequest<ClientFilter> request, ExportType exportType);

    Task<BusinessActionResult<Client>> AddClientAsync(ClientEditModel clientEditModel);

    Task<BusinessActionResult<Client>> UpdateClientAsync(ClientEditModel clientEditModel, int id);

    Task<BusinessActionResult<Client>> RemoveClientAsync(int id);
}