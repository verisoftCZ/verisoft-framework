using System.Collections.Generic;
using System.Threading.Tasks;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.AspNet.Services.Interfaces;

public interface IExportService<in TEntity>
{
    Task<ExportFileResult> ExportDataAsync(
        IEnumerable<TEntity> data,
        IEnumerable<ColumnToExport> columns,
        ExportType exportType = ExportType.Excel);
}