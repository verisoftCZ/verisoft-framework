using Verisoft.Core.AspNet.Services;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services;

public class ClientExportService(
    IClientRepository clientRepository,
    ITypeMapper typeMapper,
    IExportService<ClientListItem> exportService)
    : BaseExportService<ClientListItem, ClientFilter, ClientSpecification, ClientSort, ClientEntity, int>(typeMapper, clientRepository, exportService), IClientExportService
{
    public async Task<BusinessActionResult<ExportFileResult>> GetClientsForExportAsync(ExportDataRequest<ClientFilter> request) => await ExportDataAsync(request);

}