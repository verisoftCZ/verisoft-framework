using System;
using System.Reflection;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Extenders;

namespace Verisoft.Core.Common.Entities.Codebooks;

public class CodebookSortDetails
{
    private CodebookSortDetails(PropertyInfo codebookProperty, PropertyInfo navigationProperty, Type codebookRecordType)
    {
        CodebookProperty = codebookProperty;
        NavigationProperty = navigationProperty;
        CodebookRecordType = codebookRecordType;
    }

    public PropertyInfo CodebookProperty { get; }

    public PropertyInfo NavigationProperty { get; }

    public Type CodebookRecordType { get; }

    public static bool TryGet<TEntity, TKey>(string? propertyName,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out CodebookSortDetails? details)
    where TEntity : class, IBaseEntity<TKey>
    {
        details = null;
        if (string.IsNullOrEmpty(propertyName))
        {
            return false;
        }

        PropertyInfo? codebookPropertyInfo = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (codebookPropertyInfo == null || !codebookPropertyInfo.PropertyType.IsCodebookRecord())
        {
            return false;
        }

        Type codebookRecordType = codebookPropertyInfo.PropertyType.GetGenericArguments()[0];
        string navigationPropertyName = $"{codebookPropertyInfo.Name}{CodebookRecordEntityConstants.CodebookRecordEntityPropertySuffix}";
        PropertyInfo? navigationPropertyInfo = typeof(TEntity).GetProperty(navigationPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (navigationPropertyInfo == null)
        {
            return false;
        }

        Type expectedNavType = typeof(CodebookRecordEntity<>).MakeGenericType(codebookRecordType);
        if (!expectedNavType.IsAssignableFrom(navigationPropertyInfo.PropertyType))
        {
            return false;
        }

        details = new CodebookSortDetails(codebookPropertyInfo, navigationPropertyInfo, codebookRecordType);
        return true;
    }
}