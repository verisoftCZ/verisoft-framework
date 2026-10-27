namespace Verisoft.Core.Export.ExportResult;

public sealed class ExportFileResult : ExportJobInitiatedResult // Asi?
{
    public required Stream FileStream { get; init; }

    public required string FileType { get; init; }

    public required string FileName { get; init; }
}