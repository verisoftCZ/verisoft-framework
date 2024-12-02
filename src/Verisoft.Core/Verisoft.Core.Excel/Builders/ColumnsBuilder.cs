using System.Linq.Expressions;
using Verisoft.Core.Excel.Settings;

namespace Verisoft.Core.Excel.Builders;

public class ColumnsBuilder<TData>
{
    private readonly List<ColumnSettings<TData>> columnSettings = new();

    public ColumnBuilder<TData> Add(Expression<Func<TData, object>> expression)
    {
        var setting = new ColumnSettings<TData>() { PropertyExpression = expression };
        columnSettings.Add(setting);
        return new(setting);
    }

    internal List<ColumnSettings<TData>> Build()
    {
        return columnSettings;
    }
}
