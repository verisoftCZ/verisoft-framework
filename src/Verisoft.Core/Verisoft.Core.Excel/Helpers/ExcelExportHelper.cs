using System.Linq.Expressions;

namespace Verisoft.Core.Excel.Helpers;

internal static class ExcelExportHelper
{
    public static string GetMemberName<TData>(Expression<Func<TData, object>> expression)
    {
        if (expression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        if (expression.Body is UnaryExpression unaryExpression
            && unaryExpression.Operand is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new InvalidOperationException();
    }
}
