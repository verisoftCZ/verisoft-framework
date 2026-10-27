using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;


namespace Verisoft.Core.Export.Strategies;

public class XmlExportStrategy<TEntity> : IExportStrategy<TEntity>
{
    public bool CanHandle(ExportType exportType) => exportType == ExportType.Xml;

    public Task<ExportFileResult> ExportAsync(IEnumerable<TEntity> entities, ExportBuilder<TEntity>? settings)
    {
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<TEntity>));
        var memoryStream = new MemoryStream();

        using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            xmlSerializer.Serialize(writer, entities.ToList());
            writer.Flush();
        }

        memoryStream.Position = 0;

        return Task.FromResult(new ExportFileResult
        {
            FileStream = memoryStream,
            FileType = MimeType.Xml,
            FileName = $"{typeof(TEntity).Name.ToLower()}s.xml",
        });
    }
}
