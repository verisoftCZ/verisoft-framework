using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.Export.Strategies;

public class CsvExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    public bool CanHandle(ExportType exportType) => exportType == ExportType.Csv;

    public async Task<ExportFileResult> ExportAsync(IEnumerable<TEntity> entities, ExportBuilder<TEntity>? settings)
    {
        var memoryStream = new MemoryStream();

        await using var writer = new StreamWriter(memoryStream, new UTF8Encoding(true), leaveOpen: true);
        await using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
        });

        // Write header and records
        await csvWriter.WriteRecordsAsync(entities);

        await writer.FlushAsync();
        memoryStream.Position = 0;

        return new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Csv,
            FileName = $"{typeof(TEntity).Name.ToLower()}.csv",
        };
    }
}