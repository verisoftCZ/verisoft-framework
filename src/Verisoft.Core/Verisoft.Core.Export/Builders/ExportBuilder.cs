using Verisoft.Core.Export.Settings;

namespace Verisoft.Core.Export.Builders;

public class ExportBuilder<TData>
{
    private readonly ExportSettings<TData> exportSettings = new();

    public ExportBuilder<TData> FileName(string value)
    {
        exportSettings.FileName = value;
        return this;
    }

    public ExportBuilder<TData> Language(string? value)
    {
        exportSettings.Language = value;
        return this;
    }

    public ExportBuilder<TData> ExcelSheetName(string value)
    {
        exportSettings.ExcelSheetName = value;
        return this;
    }

    public ExportBuilder<TData> Header(Action<HeaderBuilder> configureHeader)
    {
        HeaderBuilder header = new();
        configureHeader.Invoke(header);
        exportSettings.Header = header.Build();
        return this;
    }

    public ExportBuilder<TData> Column(Action<ColumnBuilder<TData>> configureHeader)
    {
        ColumnBuilder<TData> column = new();
        configureHeader.Invoke(column);
        exportSettings.Columns.Add(column.Build());
        return this;
    }

    public ExportSettings<TData> Build() => exportSettings;
}