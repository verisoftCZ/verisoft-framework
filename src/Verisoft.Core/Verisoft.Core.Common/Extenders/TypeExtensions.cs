using System;
using System.Reflection;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Common.Extenders;

public static class TypeExtensions
{
    public static bool IsCodebookRecord(this Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(CodebookRecord<>);

    public static bool IsNullableType(this PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        if (propertyInfo.PropertyType.IsValueType)
        {
            return Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
        }

        var context = new NullabilityInfoContext();
        var nullabilityInfo = context.Create(propertyInfo);

        return nullabilityInfo.ReadState == NullabilityState.Nullable;
    }
}