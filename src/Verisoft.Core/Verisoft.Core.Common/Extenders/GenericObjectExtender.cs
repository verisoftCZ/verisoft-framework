using System;
using System.Linq;

namespace Verisoft.Core.Common.Extenders;

public static class GenericObjectExtender
{
    public static bool HasValue<T>(this string value) => !string.IsNullOrWhiteSpace(value);

    public static bool HasValue<T>(this T[] value)
    {
        if (value == null)
        {
            return false;
        }

        return Array.Exists(value, x => x != null);
    }

    public static string[] IgnoreEmptyValues(this string[] values)
    {
        return values
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
    }

    public static T[] IgnoreEmptyValues<T>(this T[] values)
    {
        return values
            .Where(x => x != null)
            .ToArray();
    }
}
