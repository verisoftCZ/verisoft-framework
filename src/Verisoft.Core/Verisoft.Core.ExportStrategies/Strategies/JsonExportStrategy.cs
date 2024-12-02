using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;

namespace Verisoft.Core.ExportStrategies.Strategies;

public class JsonExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    public bool CanHandle(ExportType exportType) => exportType == ExportType.Json;

    public Task<ExportResult.ExportResult> ExportAsync(IEnumerable<TEntity> entities)
    {
        var memoryStream = new MemoryStream();

        JsonSerializer.Serialize(memoryStream, entities, JsonOptions);

        memoryStream.Position = 0;

        return Task.FromResult<ExportResult.ExportResult>(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Json,
            FileName = $"{typeof(TEntity).Name.ToLower()}.json",
        });
    }
}