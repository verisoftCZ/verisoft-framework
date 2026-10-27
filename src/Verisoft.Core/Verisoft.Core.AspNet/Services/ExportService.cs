using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export;
using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.AspNet.Services;

public sealed class ExportService<TListData>(ExportStrategyResolver<TListData> exportStrategyResolver, ILanguageProvider languageProvider) : IExportService<TListData>
{
    private readonly ExportStrategyResolver<TListData> exportStrategyResolver = exportStrategyResolver ?? throw new ArgumentNullException(nameof(exportStrategyResolver));

    public Task<ExportFileResult> ExportDataAsync(
        IEnumerable<TListData> data,
        IEnumerable<ColumnToExport> columns,
        ExportType exportType = ExportType.Excel)
    {
        var exportableObjectDataType = data.GetType()
            .GetInterfaces()
            .FirstOrDefault(t => IsOfExpectedGenericType(t, typeof(IEnumerable<>)))
            ?.GetGenericArguments()
            .FirstOrDefault() ?? typeof(TListData);

        var exportBuilder = ConfigureExportBuilder(columns, exportableObjectDataType);
        var resolvedExport = exportStrategyResolver.Resolve(ExportType.Excel);
        return resolvedExport.ExportAsync(data, exportBuilder);
    }

    public async Task<FileStreamResult> ExportAsync<TFilter>(Func<Task<IEnumerable<TListData>>> getExportData, ExportDataRequest<TFilter> request)
    {
        ExportFileResult? exportResult = null;

        try
        {
            var exportData = await getExportData();
            exportResult = await ExportDataAsync(exportData, request.ColumnsToExport);

            return new FileStreamResult(exportResult.FileStream, exportResult.FileType) { FileDownloadName = exportResult.FileName };
        }
        catch (Exception)
        {
            if (exportResult is not null)
            {
                await exportResult.FileStream.DisposeAsync();
            }

            throw;
        }
    }

    private static IEnumerable<Action<ColumnBuilder<TListData>>> GetExportColumns(
        IEnumerable<ColumnToExport> columns,
        Type exportableObjectDataType)
    {
        var dataTypeProperties = exportableObjectDataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var columnsToProcess = columns.Any()
            ? columns
            : dataTypeProperties
                .Where(p => IsAllowedType(p.PropertyType))
                .Select(p => new ColumnToExport(p.Name, p.Name));

        return columnsToProcess
            .Select(c => ColumnToExportTableColumn(c, exportableObjectDataType, dataTypeProperties))
            .Where(c => c is not null)
            .Select(c => c!);
    }

    private static Action<ColumnBuilder<TListData>>? ColumnToExportTableColumn(
        ColumnToExport column,
        Type exportableObjectDataType,
        IReadOnlyCollection<PropertyInfo> dataTypeProperties)
    {
        var property = dataTypeProperties.FirstOrDefault(p => p.Name.Equals(column.Name, StringComparison.InvariantCultureIgnoreCase));
        if (!IsAllowedType(property?.PropertyType))
        {
            return null;
        }

        var propertyType = IsOfCodebookRecordType(property!.PropertyType) ? typeof(string) : property.PropertyType;
        var propertyExpression = CreateColumnValuePropertyExpression(exportableObjectDataType, property);
        return tableColumnBuilder =>
        {
            tableColumnBuilder.Header(column.Header)
                .Type(propertyType)
                .PropertyExpression(propertyExpression);
        };
    }

    private static bool IsAllowedType(Type? type)
    {
        if (type is null)
        {
            return false;
        }

        if (type == typeof(string) ||
            IsOfCodebookRecordType(type) ||
            IsOfExpectedGenericType(type, typeof(Nullable<>)))
        {
            return true;
        }

        return !IsIterable(type) && !IsClass(type);
    }

    private static bool IsIterable(Type type)
        => type != typeof(string) &&
           (type.IsArray || type.IsAssignableFrom(typeof(IEnumerable)) || type.IsAssignableFrom(typeof(IEnumerator)));

    private static bool IsClass(Type type)
        => type is { IsClass: true, IsValueType: false };

    private static bool IsOfCodebookRecordType(Type propertyType)
        => IsOfExpectedGenericType(propertyType, typeof(CodebookRecord<>));

    private static bool IsOfExpectedGenericType(Type propertyType, Type expectedGenericType)
        => propertyType.IsGenericType &&
           propertyType.GetGenericTypeDefinition() == expectedGenericType;

    private static Expression<Func<TListData, object?>> CreateColumnValuePropertyExpression(Type exportableObjectDataType, PropertyInfo propertyInfo)
        => IsOfCodebookRecordType(propertyInfo.PropertyType)
            ? CreateCodebookPropertyExpression(exportableObjectDataType, propertyInfo)
            : CreatePropertyExpression(exportableObjectDataType, propertyInfo);

    private static Expression<Func<TListData, object?>> CreatePropertyExpression(Type exportableObjectDataType, PropertyInfo propertyInfo)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(object), "x");
        UnaryExpression castParameter = Expression.Convert(parameter, exportableObjectDataType);
        MemberExpression propertyAccess = Expression.Property(castParameter, propertyInfo);
        UnaryExpression castPropertyAccess = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<TListData, object?>>(castPropertyAccess, parameter);
    }

    private static Expression<Func<TListData, object?>> CreateCodebookPropertyExpression(Type exportableObjectDataType, PropertyInfo propertyInfo)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(object), "x");
        UnaryExpression castParameter = Expression.Convert(parameter, exportableObjectDataType);
        MemberExpression codebookProperty = Expression.Property(castParameter, propertyInfo);

        ConstantExpression nullConstant = Expression.Constant(null);
        BinaryExpression codebookNotNull = Expression.NotEqual(codebookProperty, nullConstant);

        MemberExpression translationProperty = Expression.Property(codebookProperty, nameof(CodebookRecord<CodebookRecordType>.Translation));
        MemberExpression stringValueProperty = Expression.Property(codebookProperty, nameof(CodebookRecord<CodebookRecordType>.StringValue));

        MethodInfo isNullOrWhitespaceMethod = typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), [typeof(string)])
            ?? throw new UnreachableException(".NET method string.IsNullOrWhiteSpace not found.");
        MethodCallExpression isTranslationNullOrWhitespaceCall = Expression.Call(isNullOrWhitespaceMethod, translationProperty);
        ConditionalExpression nullTranslationFallbackCondition = Expression.Condition(
            isTranslationNullOrWhitespaceCall,
            stringValueProperty,
            translationProperty);

        UnaryExpression castNullTranslationFallback = Expression.Convert(nullTranslationFallbackCondition, typeof(object));
        ConditionalExpression nullCodebookFallbackCondition = Expression.Condition(
            codebookNotNull,
            castNullTranslationFallback,
            nullConstant);

        return Expression.Lambda<Func<TListData, object?>>(nullCodebookFallbackCondition, parameter);
    }

    private ExportBuilder<TListData> ConfigureExportBuilder(
        IEnumerable<ColumnToExport> columns,
        Type exportableObjectDataType)
    {
        ExportBuilder<TListData> exportBuilder = new();
        exportBuilder.FileName("export")
            .Language(languageProvider.PreferredLanguageCode)
            .ExcelSheetName("export")
            .Header(header => header.Background($"#{30:X2}{144:X2}{255:X2}"));

        foreach (var configureTableColumn in GetExportColumns(columns, exportableObjectDataType))
        {
            exportBuilder.Column(configureTableColumn);
        }

        return exportBuilder;
    }
}