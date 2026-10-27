using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.Excel;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.Export.Strategies;

public class ExcelExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    public bool CanHandle(ExportType exportType) => exportType == ExportType.Excel;

    public Task<ExportFileResult> ExportAsync(IEnumerable<TEntity> entities, ExportBuilder<TEntity>? settings)
    {
        var memoryStream = new MemoryStream();
        var exportSettings = settings?.Build();
        new ExcelExport<TEntity>(entities, exportSettings).Export(memoryStream);

        memoryStream.Position = 0;
        var fileName = exportSettings?.FileName ?? typeof(TEntity).Name.ToLower();
        return Task.FromResult(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Excel,
            FileName = $"{fileName}.xlsx",
        });
    }
}