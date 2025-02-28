using System;
using System.Linq;
using System.Reflection;

namespace Verisoft.Core.Common.Helpers;

public static class ObjectReflectionHelper
{
    public static bool HasAnyProperty(object obj)
    {
        var properties = obj.GetType().GetProperties();
        return properties.Any(prop => prop.GetValue(obj) is not null);
    }

    public static void MapProperties(object source, object target)
    {
        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name);

        foreach (var sourceProperty in sourceProperties)
        {
            if (sourceProperty.PropertyType.IsClass
                 && sourceProperty.PropertyType != typeof(string)
                 && targetProperties.TryGetValue(sourceProperty.Name, out var nestedTargetProperty)
                 && nestedTargetProperty.PropertyType.IsClass
                 && nestedTargetProperty.CanWrite)
            {
                var nestedSourceValue = sourceProperty.GetValue(source);
                if (nestedSourceValue is not null)
                {
                    var nestedTargetValue = nestedTargetProperty.GetValue(target);
                    if (nestedTargetValue is null)
                    {
                        nestedTargetValue = Activator.CreateInstance(nestedTargetProperty.PropertyType);
                        nestedTargetProperty.SetValue(target, nestedTargetValue);
                    }

                    MapProperties(nestedSourceValue, nestedTargetValue);
                }
            }
            else if (targetProperties.TryGetValue(sourceProperty.Name, out var targetProperty)
            && targetProperty.PropertyType == sourceProperty.PropertyType
            && targetProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);
                targetProperty.SetValue(target, value);
            }
        }
    }
}
