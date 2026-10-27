using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.ExportResult;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IClientExportService : IBaseExportService<ClientListItem, ClientFilter, int>
{
    Task<BusinessActionResult<ExportFileResult>> GetClientsForExportAsync(ExportDataRequest<ClientFilter> request);
}