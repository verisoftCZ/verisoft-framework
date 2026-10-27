using System.Threading.Tasks;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.AspNet.Services.Interfaces;

public interface IBaseExportService<TListItem, TFilter, TKey>
{
    Task<ExportFileResult> ExportDataAsync(ExportDataRequest<TFilter> request);
}