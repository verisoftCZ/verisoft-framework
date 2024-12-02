namespace Verisoft.Core.Excel.Settings;

internal class ExportSettings<TData>
{
    public string? SheetName { get; set; }

    public List<ColumnSettings<TData>>? Columns { get; set; }

    public HeaderSettings? Header { get; set; }
}
