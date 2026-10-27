namespace Verisoft.Core.Export.Settings;

public class ExportSettings<TData>
{
    public string? FileName { get; set; }

    public string? Language { get; set; }

    public string? ExcelSheetName { get; set; }

    public HeaderSettings? Header { get; set; }

    public ICollection<ColumnSettings<TData>> Columns { get; set; } = [];
}
