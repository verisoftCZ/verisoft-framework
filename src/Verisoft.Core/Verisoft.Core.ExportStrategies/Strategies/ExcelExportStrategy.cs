using Verisoft.Core.Excel;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;

namespace Verisoft.Core.ExportStrategies.Strategies;

public class ExcelExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    public bool CanHandle(ExportType exportType) => exportType == ExportType.Excel;

    public Task<ExportResult.ExportResult> ExportAsync(IEnumerable<TEntity> entities)
    {
        var memoryStream = new MemoryStream();
        new ExcelExport<TEntity>(entities).Export(memoryStream);

        memoryStream.Position = 0;

        return Task.FromResult<ExportResult.ExportResult>(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Excel,
            FileName = $"{typeof(TEntity).Name.ToLower()}.xlsx",
        });
    }
}