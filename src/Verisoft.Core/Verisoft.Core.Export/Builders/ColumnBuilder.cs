using System.Linq.Expressions;
using Verisoft.Core.Export.Settings;

namespace Verisoft.Core.Export.Builders;

public class ColumnBuilder<TData>
{
    private readonly ColumnSettings<TData> columnSettings = new();

    public ColumnBuilder<TData> PropertyExpression(Expression<Func<TData, object?>> propertyExpression)
    {
        columnSettings.PropertyExpression = propertyExpression;
        return this;
    }

    public ColumnBuilder<TData> Type(Type type)
    {
        columnSettings.Type = type;
        return this;
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

    public ColumnSettings<TData> Build() => columnSettings;
}
