using Verisoft.Core.Excel.Settings;

namespace Verisoft.Core.Excel.Builders;

public class ColumnBuilder<TData>
{
    private readonly ColumnSettings<TData> columnSettings;

    public ColumnBuilder(ColumnSettings<TData> columnSettings)
    {
        this.columnSettings = columnSettings;
    }

    public ColumnBuilder<TData> Header(string header)
    {
        columnSettings.HeaderText = header;
        return this;
    }

    public ColumnBuilder<TData> Width(double width)
    {
        columnSettings.Width = width;
        return this;
    }
}
