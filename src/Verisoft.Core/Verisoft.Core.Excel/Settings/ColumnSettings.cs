using System.Linq.Expressions;

namespace Verisoft.Core.Excel.Settings;

public class ColumnSettings<TData>
{
    required public Expression<Func<TData, object>> PropertyExpression { get; set; } = default!;

    public string? HeaderText { get; set; }

    public double? Width { get; set; }
}