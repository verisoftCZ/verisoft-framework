using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;

namespace Verisoft.Core.ExportStrategies.Strategies;

public class XmlExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    public bool CanHandle(ExportType exportType) => exportType == ExportType.Xml;

    public Task<ExportResult.ExportResult> ExportAsync(IEnumerable<TEntity> entities)
    {
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<TEntity>));
        var memoryStream = new MemoryStream();

        using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            xmlSerializer.Serialize(writer, entities.ToList());
            writer.Flush();
        }

        memoryStream.Position = 0;

        return Task.FromResult<ExportResult.ExportResult>(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Xml,
            FileName = $"{typeof(TEntity).Name.ToLower()}s.xml",
        });
    }
}
