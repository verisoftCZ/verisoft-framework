using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Verisoft.Core.Common.Extensions;

public static class EnumAttributeExtensions
{
    private static readonly ConcurrentDictionary<(Enum Value, Type AttributeType), Attribute?> Cache = new();

    public static bool TryGetAttribute<TEnum, TAttribute>(this TEnum value, out TAttribute? attribute)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        var cached = Cache.GetOrAdd((value, typeof(TAttribute)), static key =>
        {
            var fieldInfo = key.Value.GetType().GetField(key.Value.ToString());
            return fieldInfo?.GetCustomAttribute(key.AttributeType);
        });

        attribute = cached as TAttribute;
        return attribute is not null;
    }

    public static TAttribute? GetAttribute<TEnum, TAttribute>(this TEnum value)
        where TEnum : struct, Enum
        where TAttribute : Attribute
        => value.TryGetAttribute<TEnum, TAttribute>(out var attribute) ? attribute : null;

    public static bool TryGetDisplayName<TEnum>(this TEnum value, out string? displayName)
        where TEnum : struct, Enum
    {
        displayName = value.TryGetAttribute<TEnum, DisplayAttribute>(out var attribute)
            ? attribute!.GetName()
            : null;

        return displayName is not null;
    }

    public static string GetDisplayName<TEnum>(this TEnum value)
        where TEnum : struct, Enum
        => value.TryGetDisplayName(out var displayName) ? displayName! : value.ToString();
}