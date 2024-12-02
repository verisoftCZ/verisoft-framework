using System.Text.Json.Serialization;

namespace Verisoft.Core.ExportStrategies.ExportResult;

public class ExportFileResult : ExportResult
{
    [JsonIgnore]
    public Stream FileStream { get; set; }

    public string FileType { get; set; }

    public string FileName { get; set; }
}