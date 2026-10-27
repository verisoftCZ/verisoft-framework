using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Constants;
using Verisoft.Core.Codebooks.Common.Extensions;
using Verisoft.Core.Codebooks.Common.Helpers;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Codebooks.Context;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.Core.Codebooks.Repositories;

public abstract class CodebookBaseRepository<TEntity, TKey>(
    ICodebookDbContext unitOfWork,
    ILanguageProvider languageProvider)
    : EntityFrameworkRepositoryBase<TEntity, TKey>(unitOfWork),
    ICodebookBaseRepository<TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>
{
    private readonly ILanguageProvider languageProvider = languageProvider ?? throw new ArgumentNullException(nameof(languageProvider));

    public new virtual Task<TEntity?> GetAsync(TKey id)
    {
        return GetQueryable().FirstOrDefaultAsync((t) => t.Id!.Equals(id));
    }

    public virtual async Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(
        int offset,
        int limit,
        Expression<Func<TEntity, bool>>? filter,
        SortDefinition? sort)
        where TSort : BaseSort<TEntity>, new()
    {
        var query = FilterData(filter);
        return await GetPagedSortAsync<TSort>(query, offset, limit, sort);
    }

    public async Task<IEnumerable<TEntity>> GetFilteredSortForExportAsync<TSort>(
        Expression<Func<TEntity, bool>>? filter,
        SortDefinition? sort)
        where TSort : BaseSort<TEntity>, new()
    {
        var query = GetQueryableForExport();
        query = ApplyFilter(query, filter);
        return await ApplySort<TSort>(query, sort).ToArrayAsync();
    }

    public override void Remove(TEntity entity)
    {
        GetDbSet().Remove(entity);
    }

    public override void RemoveRange(IEnumerable<TEntity> entities)
    {
        GetDbSet().RemoveRange(entities);
    }

    public override void Update(TEntity entity)
    {
        GetDbSet().Update(entity);
    }

    public override void UpdateRange(IEnumerable<TEntity> entities)
    {
        GetDbSet().UpdateRange(entities);
    }

    protected override DbSet<TEntity> GetDbSet()
        => unitOfWork.Set<TEntity>();

    protected virtual async Task<(IEnumerable<TEntity> Items, int Count)> GetPagedSortAsync<TSort>(IQueryable<TEntity> query, int offset, int limit, SortDefinition? sort)
        where TSort : BaseSort<TEntity>, new()
    {
        var count = await query.CountAsync();
        var data = await ApplySort<TSort>(query, sort)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        return (data, count);
    }

    protected virtual IQueryable<TEntity> GetQueryableForExport()
        => GetQueryable();

    private static Expression<Func<TEntity, string?>> BuildTranslationSortValueExpression(
        CodebookSortDetails codebookDetails,
        string preferredLanguageCode,
        string defaultLanguageCode)
    {
        ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity), "x");
        Type codebookRecordType = codebookDetails.CodebookRecordType;
        PropertyInfo navigationPropertyInfo = codebookDetails.NavigationProperty;
        Type navigationPropertyConcreteType = typeof(CodebookRecordEntity<>).MakeGenericType(codebookRecordType);
        Type translationEntityConcreteType = typeof(CodebookRecordTranslationEntity<>).MakeGenericType(codebookRecordType);
        Expression navigationPropertyAccess = Expression.MakeMemberAccess(entityParameter, navigationPropertyInfo);
        PropertyInfo? translationsPropertyInfo = navigationPropertyConcreteType.GetProperty(CodebookRecordEntityConstants.Translations)
            ?? throw new MissingMemberException(navigationPropertyConcreteType.Name, CodebookRecordEntityConstants.Translations);

        Expression translationsCollectionAccess = Expression.MakeMemberAccess(navigationPropertyAccess, translationsPropertyInfo);

        MethodInfo asQueryableGeneric = MethodInfoHelper.QueryableAsQueryableMethodDefinition.MakeGenericMethod(translationEntityConcreteType);
        Expression queryableTranslationsAccess = Expression.Call(null, asQueryableGeneric, translationsCollectionAccess);

        ParameterExpression translationParam = Expression.Parameter(translationEntityConcreteType, "t");
        MemberExpression langCodeAccess = translationParam.GetPropertyAccessExpression(CodebookRecordTranslationEntityConstants.LanguageCode, translationEntityConcreteType);
        MemberExpression valueAccess = translationParam.GetPropertyAccessExpression(CodebookRecordTranslationEntityConstants.Value, translationEntityConcreteType);
        ConstantExpression preferredLangConst = Expression.Constant(preferredLanguageCode);
        ConstantExpression defaultLangConst = Expression.Constant(defaultLanguageCode);
        MethodInfo whereGeneric = MethodInfoHelper.GetQueryableWhereMethodWithExpression.MakeGenericMethod(translationEntityConcreteType);
        MethodInfo selectGeneric = MethodInfoHelper.GetQueryableSelectMethodWithExpression.MakeGenericMethod(translationEntityConcreteType, typeof(string));
        MethodInfo firstOrDefaultGeneric = MethodInfoHelper.GetQueryableFirstOrDefaultMethod(1).MakeGenericMethod(typeof(string));

        LambdaExpression valueSelector = Expression.Lambda(valueAccess, translationParam);

        // Predikát pro filtraci podle preferovaného jazyka
        LambdaExpression prefLangPredicate = Expression.Lambda(Expression.Equal(langCodeAccess, preferredLangConst), translationParam);
        Expression prefWhereCall = Expression.Call(null, whereGeneric, queryableTranslationsAccess, prefLangPredicate);
        Expression prefSelectCall = Expression.Call(null, selectGeneric, prefWhereCall, valueSelector);
        Expression prefFirstOrDefaultCall = Expression.Call(null, firstOrDefaultGeneric, prefSelectCall);

        // Predikát pro filtraci podle výchozího jazyka
        LambdaExpression defLangPredicate = Expression.Lambda(Expression.Equal(langCodeAccess, defaultLangConst), translationParam);
        Expression defWhereCall = Expression.Call(null, whereGeneric, queryableTranslationsAccess, defLangPredicate);
        Expression defSelectCall = Expression.Call(null, selectGeneric, defWhereCall, valueSelector);
        Expression defFirstOrDefaultCall = Expression.Call(null, firstOrDefaultGeneric, defSelectCall);

        Expression coalescedValue = Expression.Coalesce(prefFirstOrDefaultCall, defFirstOrDefaultCall);
        Expression finalValueExpression = Expression.Condition(
            Expression.NotEqual(navigationPropertyAccess, Expression.Constant(null, navigationPropertyInfo.PropertyType)),
            coalescedValue,
            Expression.Constant(null, typeof(string)));

        return Expression.Lambda<Func<TEntity, string?>>(finalValueExpression, entityParameter);
    }

    private IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, Expression<Func<TEntity, bool>>? filter)
        => filter is null ? query : query.Where(filter);

    private IQueryable<TEntity> ApplySort<TSort>(IQueryable<TEntity> query, SortDefinition? sort)
        where TSort : BaseSort<TEntity>, new()
    {
        var propertyName = sort?.Field;
        var isAscending = IsAscending(sort);
        var sortProvider = new TSort();

        return CodebookSortDetails.TryGet<TEntity, TKey>(propertyName, out var codebookDetails)
            ? ApplyCodebookSort(query, codebookDetails, isAscending, sortProvider)
            : ApplySort<TSort>(query, sort is null ? null : [sort]);
    }

    private IQueryable<TEntity> ApplyCodebookSort<TSort>(IQueryable<TEntity> query, CodebookSortDetails codebookDetails, bool isAscending, TSort sortProvider)
        where TSort : BaseSort<TEntity>
    {
        var preferredLanguage = languageProvider.PreferredLanguageCode ?? ILanguageProvider.DefaultLanguageCode;
        var sortValueExpression = BuildTranslationSortValueExpression(codebookDetails, preferredLanguage, ILanguageProvider.DefaultLanguageCode);

        var ordered = isAscending
            ? query.OrderBy(sortValueExpression)
            : query.OrderByDescending(sortValueExpression);

        return ApplyThenSorts(
            ordered,
            sortProvider.DefaultSorts);
    }

    private static IOrderedQueryable<TEntity> ApplyThenSorts(
        IOrderedQueryable<TEntity> query,
        IEnumerable<SortClause<TEntity>> sorts)
    {
        foreach (var sort in sorts)
        {
            query = sort.Direction == SortDirection.Asc
                ? query.ThenBy(sort.Expression)
                : query.ThenByDescending(sort.Expression);
        }

        return query;
    }
}