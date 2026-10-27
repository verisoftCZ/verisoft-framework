using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;


namespace Verisoft.Core.Export.Strategies;

public class JsonExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    public bool CanHandle(ExportType exportType) => exportType == ExportType.Json;

    public Task<ExportFileResult> ExportAsync(IEnumerable<TEntity> entities, ExportBuilder<TEntity>? settings)
    {
        var memoryStream = new MemoryStream();

        JsonSerializer.Serialize(memoryStream, entities, JsonOptions);

        memoryStream.Position = 0;

        return Task.FromResult(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Json,
            FileName = $"{typeof(TEntity).Name.ToLower()}.json",
        });
    }
}