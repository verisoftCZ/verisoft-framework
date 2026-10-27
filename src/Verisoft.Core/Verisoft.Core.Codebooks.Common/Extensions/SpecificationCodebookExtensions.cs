using System.Linq.Expressions;
using System.Reflection;
using Verisoft.Core.Codebooks.Common.Helpers;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Codebooks.Common.Extensions;

public static class SpecificationCodebookExtensions
{
    private enum ComparisonType
    {
        Equals,
        NotEquals,
        Contains,
        NotContains,
    }

    public static Specification<TEntity> BuildAndEqualsCodebookSpecification<TEntity, TProperty, TCodebookRecordType>(
        this Specification<TEntity>? specification,
        Expression<Func<TEntity, TProperty>> propertySelector,
        CodebookRecord<TCodebookRecordType>? value,
        Func<string, string>? foreignKeyPropertyNamingConvention = null)
        where TEntity : class
        where TProperty : CodebookRecord<TCodebookRecordType>
        where TCodebookRecordType : CodebookRecordType
    {
        return AddCodebookCondition(specification, propertySelector, value?.StringValue, ComparisonType.Equals, foreignKeyPropertyNamingConvention);
    }

    public static Specification<TEntity> BuildAndNotEqualsCodebookSpecification<TEntity, TProperty, TCodebookRecordType>(
        this Specification<TEntity>? specification,
        Expression<Func<TEntity, TProperty>> propertySelector,
        CodebookRecord<TCodebookRecordType>? value,
        Func<string, string>? foreignKeyPropertyNamingConvention = null)
        where TEntity : class
        where TProperty : CodebookRecord<TCodebookRecordType>
        where TCodebookRecordType : CodebookRecordType
    {
        return AddCodebookCondition(specification, propertySelector, value?.StringValue, ComparisonType.NotEquals, foreignKeyPropertyNamingConvention);
    }

    public static Specification<TEntity> BuildAndContainsCodebookSpecification<TEntity, TProperty, TCodebookRecordType>(
        this Specification<TEntity>? specification,
        Expression<Func<TEntity, TProperty?>> propertySelector,
        IEnumerable<CodebookRecord<TCodebookRecordType>>? values,
        Func<string, string>? foreignKeyPropertyNamingConvention = null)
        where TEntity : class
        where TProperty : CodebookRecord<TCodebookRecordType>
        where TCodebookRecordType : CodebookRecordType
    {
        var stringValues = values?.Select(v => v?.StringValue).Where(s => s != null).ToList();
        return AddCodebookCondition(specification, propertySelector, stringValues, ComparisonType.Contains, foreignKeyPropertyNamingConvention);
    }

    public static Specification<TEntity> BuildAndNotContainsCodebookSpecification<TEntity, TProperty, TCodebookRecordType>(
        this Specification<TEntity>? specification,
        Expression<Func<TEntity, TProperty>> propertySelector,
        IEnumerable<CodebookRecord<TCodebookRecordType>>? values,
        Func<string, string>? foreignKeyPropertyNamingConvention = null)
        where TEntity : class
        where TProperty : CodebookRecord<TCodebookRecordType>
        where TCodebookRecordType : CodebookRecordType
    {
        var stringValues = values?.Select(v => v?.StringValue).Where(s => s != null).ToList();
        return AddCodebookCondition(specification, propertySelector, stringValues, ComparisonType.NotContains, foreignKeyPropertyNamingConvention);
    }

    public static DirectSpecification<TEntity> CreateCodebookTranslationFilterSpecification<TEntity, TCodebookRecordType>(
        this string? searchTerm,
        Expression<Func<TEntity, CodebookRecordEntity<TCodebookRecordType>?>> navigationPropertySelector,
        string? preferredLanguageCode)
        where TEntity : class
        where TCodebookRecordType : CodebookRecordType, new()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new DirectSpecification<TEntity>(e => true);
        }

        var finalLambda = searchTerm.CreateCodebookTranslationFilterExpression(navigationPropertySelector, preferredLanguageCode);

        return new DirectSpecification<TEntity>(finalLambda);
    }

    public static DirectSpecification<TEntity> CreateCodebookTranslationFilterSpecification<TEntity, TCollectionItem, TCodebookRecordType>(
        this string? searchTerm,
        Expression<Func<TEntity, IEnumerable<TCollectionItem>>> collectionSelector,
        Expression<Func<TCollectionItem, CodebookRecordEntity<TCodebookRecordType>?>> navigationPropertySelector,
        string? preferredLanguageCode)
        where TEntity : class
        where TCollectionItem : class
        where TCodebookRecordType : CodebookRecordType, new()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new DirectSpecification<TEntity>(e => true);
        }

        preferredLanguageCode ??= ILanguageProvider.DefaultLanguageCode;

        var entityParameter = Expression.Parameter(typeof(TEntity), "e");

        var replacedCollectionSelectorBody = ParameterRebinder
            .ReplaceParameters(
                new Dictionary<ParameterExpression, ParameterExpression>
                {
                    { collectionSelector.Parameters[0], entityParameter },
                },
                collectionSelector.Body);

        var collectionAccess = replacedCollectionSelectorBody;

        var innerFilter = searchTerm.CreateCodebookTranslationFilterExpression(navigationPropertySelector, preferredLanguageCode);

        var itemParameter = Expression.Parameter(typeof(TCollectionItem), "item");

        var replacedInnerFilterBody = ParameterRebinder
            .ReplaceParameters(
                new Dictionary<ParameterExpression, ParameterExpression>
                {
                    { innerFilter.Parameters[0], itemParameter },
                },
                innerFilter.Body);

        var itemPredicate = Expression.Lambda<Func<TCollectionItem, bool>>(replacedInnerFilterBody, itemParameter);

        var anyMethodGeneric = MethodInfoHelper.EnumerableAnyMethodDefinition.MakeGenericMethod(typeof(TCollectionItem));
        var anyCall = Expression.Call(null, anyMethodGeneric, collectionAccess, itemPredicate);

        var finalLambda = Expression.Lambda<Func<TEntity, bool>>(anyCall, entityParameter);

        return new DirectSpecification<TEntity>(finalLambda);
    }

    public static Expression<Func<TEntity, bool>> CreateCodebookTranslationFilterExpression<TEntity, TCodebookRecordType>(
        this string? searchTerm,
        Expression<Func<TEntity, CodebookRecordEntity<TCodebookRecordType>?>> navigationPropertySelector,
        string? preferredLanguageCode)
        where TEntity : class
        where TCodebookRecordType : CodebookRecordType, new()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Expression<Func<TEntity, bool>> filter = item => true;
            return filter;
        }

        ParameterExpression entityParameter = navigationPropertySelector.Parameters[0];
        if (navigationPropertySelector.Body is not MemberExpression navigationPropertyAccess)
        {
            throw new ArgumentException("The navigation property selector must be a MemberExpression.", nameof(navigationPropertySelector));
        }

        preferredLanguageCode ??= ILanguageProvider.DefaultLanguageCode;

        Type navigationPropertyType = typeof(CodebookRecordEntity<TCodebookRecordType>);
        Type translationEntityType = typeof(CodebookRecordTranslationEntity<TCodebookRecordType>);

        PropertyInfo? translationsPropertyInfo = navigationPropertyType.GetProperty(nameof(CodebookRecordEntity<TCodebookRecordType>.Translations), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new MissingMemberException(navigationPropertyType.FullName, nameof(CodebookRecordEntity<TCodebookRecordType>.Translations));

        Expression translationsCollectionAccess = Expression.MakeMemberAccess(navigationPropertyAccess, translationsPropertyInfo);

        ParameterExpression translationParam = Expression.Parameter(translationEntityType, "t");

        MemberExpression langCodeAccess = translationParam.GetPropertyAccessExpression(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.LanguageCode), translationEntityType);
        MemberExpression valueAccess = translationParam.GetPropertyAccessExpression(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.Value), translationEntityType);

        ConstantExpression searchTermConst = Expression.Constant(searchTerm);
        ConstantExpression preferredLangConst = Expression.Constant(preferredLanguageCode);
        ConstantExpression defaultLangConst = Expression.Constant(ILanguageProvider.DefaultLanguageCode);

        Expression valueContainsSearchTerm = Expression.AndAlso(
                Expression.NotEqual(valueAccess, Expression.Constant(null, typeof(string))),
                Expression.Call(valueAccess, MethodInfoHelper.StringContainsMethod, searchTermConst));

        MethodInfo anyMethodGeneric = MethodInfoHelper.EnumerableAnyMethodDefinition.MakeGenericMethod(translationEntityType);

        Expression predicateBody1 = Expression.AndAlso(Expression.Equal(langCodeAccess, preferredLangConst), valueContainsSearchTerm);
        Expression anyCall1 = Expression.Call(null, anyMethodGeneric, translationsCollectionAccess, Expression.Lambda(predicateBody1, translationParam));

        Expression predicateBody2a = Expression.Equal(langCodeAccess, preferredLangConst);
        Expression anyCall2a = Expression.Call(null, anyMethodGeneric, translationsCollectionAccess, Expression.Lambda(predicateBody2a, translationParam));
        Expression notAnyCall2a = Expression.Not(anyCall2a);

        Expression predicateBody2b = Expression.AndAlso(Expression.Equal(langCodeAccess, defaultLangConst), valueContainsSearchTerm);
        Expression anyCall2b = Expression.Call(null, anyMethodGeneric, translationsCollectionAccess, Expression.Lambda(predicateBody2b, translationParam));

        Expression combinedCondition2 = Expression.AndAlso(notAnyCall2a, anyCall2b);

        Expression filterLogic = Expression.OrElse(anyCall1, combinedCondition2);

        Expression finalBody = Expression.AndAlso(
            Expression.NotEqual(navigationPropertyAccess, Expression.Constant(null, navigationPropertyType)),
            filterLogic);

        var finalLambda = Expression.Lambda<Func<TEntity, bool>>(finalBody, entityParameter);

        return finalLambda;
    }

    private static string DefaultForeignKeyPropertyConvention(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return string.Empty;
        }

        return $"{propertyName}{CodebookRecordEntityConstants.CodebookRecordForeignKeyPropertySuffix}";
    }

    private static Specification<TEntity> AddCodebookCondition<TEntity, TProperty>(
         Specification<TEntity>? specification,
         Expression<Func<TEntity, TProperty>> propertySelector,
         string? value,
         ComparisonType comparisonType,
         Func<string, string>? foreignKeyPropertyNamingConvention)
         where TEntity : class
    {
        var (parameter, foreignKeyPropertyExpression) = GetForeignKeyPropertyExpression(propertySelector, foreignKeyPropertyNamingConvention);

        Expression comparison;
        var valueConstant = Expression.Constant(value, typeof(string));

        if (comparisonType == ComparisonType.Equals)
        {
            comparison = Expression.Equal(foreignKeyPropertyExpression, valueConstant);
        }
        else if (comparisonType == ComparisonType.NotEquals)
        {
            comparison = Expression.NotEqual(foreignKeyPropertyExpression, valueConstant);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(comparisonType), "This method supports only Equals and NotEquals.");
        }

        var lambda = Expression.Lambda<Func<TEntity, bool>>(comparison, parameter);
        var newSpec = new DirectSpecification<TEntity>(lambda);

        return specification == null ? newSpec : specification && newSpec;
    }

    private static Specification<TEntity> AddCodebookCondition<TEntity, TProperty>(
        Specification<TEntity>? specification,
        Expression<Func<TEntity, TProperty>> propertySelector,
        List<string?>? stringValues,
        ComparisonType comparisonType,
        Func<string, string>? foreignKeyPropertyNamingConvention)
        where TEntity : class
    {
        if (stringValues?.Count is null or 0)
        {
            return specification!;
        }

        var (parameter, foreignKeyPropertyExpression) = GetForeignKeyPropertyExpression(propertySelector, foreignKeyPropertyNamingConvention);

        Expression comparison;
        var valuesConstant = Expression.Constant(stringValues, typeof(List<string>));

        Expression containsCall = Expression.Call(
            null,
            MethodInfoHelper.EnumerableContainsMethod,
            valuesConstant,
            foreignKeyPropertyExpression);

        if (comparisonType == ComparisonType.Contains)
        {
            comparison = containsCall;
        }
        else if (comparisonType == ComparisonType.NotContains)
        {
            comparison = Expression.Not(containsCall);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(comparisonType), "This method supports only Contains and NotContains.");
        }

        var lambda = Expression.Lambda<Func<TEntity, bool>>(comparison, parameter);
        var newSpec = new DirectSpecification<TEntity>(lambda);

        return specification == null ? newSpec : specification && newSpec;
    }

    private static (ParameterExpression Parameter, Expression ForeignKeyPropertyExpression) GetForeignKeyPropertyExpression<TEntity, TProperty>(
         Expression<Func<TEntity, TProperty>> propertySelector,
         Func<string, string>? foreignKeyPropertyNamingConvention)
    {
        if (propertySelector.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("The property selector expression must be a MemberExpression (e.g., x => x.PropertyName).", nameof(propertySelector));
        }

        if (memberExpression.Member is not PropertyInfo propertyInfo)
        {
            throw new ArgumentException("The property selector expression must refer to a property.", nameof(propertySelector));
        }

        string propertyName = propertyInfo.Name;
        ParameterExpression entityParameter = propertySelector.Parameters[0];

        var convention = foreignKeyPropertyNamingConvention ?? DefaultForeignKeyPropertyConvention;
        string foreignKeyPropertyName = convention(propertyName);
        if (string.IsNullOrEmpty(foreignKeyPropertyName))
        {
            throw new InvalidOperationException($"Cannot deduce the foreign key property name for property '{propertyName}' using the provided convention.");
        }

        var propertyCall = Expression.Call(
            null,
            MethodInfoHelper.EfPropertyMethod,
            entityParameter,
            Expression.Constant(foreignKeyPropertyName));

        return (entityParameter, propertyCall);
    }
}