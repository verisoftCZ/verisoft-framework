using System.Linq.Expressions;
using System.Reflection;

namespace Verisoft.Core.Codebooks.Common.Extensions;

public static class ExpressionExtensions
{
    public static MemberExpression GetPropertyAccessExpression(this Expression parameterOrInstance, string propertyName, Type targetType)
    {
        PropertyInfo? propInfo = targetType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new ArgumentException($"Property '{propertyName}' was not found on type '{targetType.Name}'.");

        if (!propInfo.CanRead)
        {
            throw new ArgumentException($"Property '{propertyName}' on type '{targetType.Name}' is not readable.");
        }

        return Expression.Property(parameterOrInstance, propInfo.Name);
    }
}