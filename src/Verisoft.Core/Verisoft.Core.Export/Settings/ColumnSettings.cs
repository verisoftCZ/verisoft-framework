using System.Linq.Expressions;

namespace Verisoft.Core.Export.Settings;

public class ColumnSettings<TData>
{
    public Expression<Func<TData, object?>>? PropertyExpression { get; set; }

    public Type? Type { get; set; }

    public string? HeaderText { get; set; }

    public double? Width { get; set; }
}