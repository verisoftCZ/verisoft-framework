using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ClosedXML.Excel;
using Verisoft.Core.Export.Settings;

namespace Verisoft.Core.Export.Excel;

public class ExcelExport<TData>(IEnumerable<TData> data, ExportSettings<TData>? settings)
{
    private const int HeaderRow = 1;
    private const string DefaultHeaderColor = "#FF950E";
    private const string DefaultSheetName = "Sheet1";
    private const string DefaultLanguageCode = "en";

    private readonly ExportSettings<TData> settings = settings ?? new();

    public void Export(Stream stream)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(settings.ExcelSheetName ?? DefaultSheetName);
        if (settings.Columns.Count <= 0)
        {
            settings.Columns = GenerateColumnsFromGenericArgument();
        }

        CreateTable(worksheet);
        SetTableHeader(worksheet);
        WriteData(worksheet);

        workbook.SaveAs(stream);
    }

    private static List<ColumnSettings<TData>> GenerateColumnsFromGenericArgument()
    {
        List<ColumnSettings<TData>> columns = new();

        Type type = typeof(TData);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            ParameterExpression parameter = Expression.Parameter(type, "x");
            MemberExpression propertyAccess = Expression.Property(parameter, property);

            UnaryExpression convertedExpression = Expression.Convert(propertyAccess, typeof(object));

            var lambda = Expression.Lambda<Func<TData, object?>>(convertedExpression, parameter);

            columns.Add(new ColumnSettings<TData>() { PropertyExpression = lambda });
        }

        return columns;
    }

    private static string? GetMemberName(Expression<Func<TData, object?>>? expression)
        => expression?.Body switch
        {
            MemberExpression member => member.Member.Name,
            UnaryExpression { Operand: MemberExpression memberExpression } => memberExpression.Member.Name,
            _ => null,
        };

    private static string? GetTypeNameFromColumn(ColumnSettings<TData> column)
    {
        if (column.Type is null)
        {
            return null;
        }

        if (column.Type.IsGenericType && column.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return column.Type.GetGenericArguments().FirstOrDefault()?.Name;
        }

        return column.Type.Name;
    }

    private static void SetCellFormat(IXLCell cell, string? columnTypeName)
    {
        if (cell.Value.IsBlank)
        {
            return;
        }

        switch (columnTypeName)
        {
            case nameof(String):
            case nameof(Boolean):
                cell.Style.NumberFormat.Format = "@";
                return;

            case nameof(DateTime):
            case nameof(DateTimeOffset):
                cell.Style.DateFormat.Format = "yyyy-mm-dd hh:mm:ss";
                return;

            case nameof(DateOnly):
                cell.Style.DateFormat.Format = "yyyy-mm-dd";
                return;

            case nameof(Decimal):
            case nameof(Double):
            case nameof(Single):
                cell.Style.NumberFormat.Format = "#,##0.000";
                return;

            case nameof(Int16):
            case nameof(Int32):
            case nameof(Int64):
                cell.Style.NumberFormat.Format = "#,##0";
                return;

            default:
                return;
        }
    }

    private void CreateTable(IXLWorksheet worksheet)
    {
        if (!AreSettingsValidForGeneration())
        {
            return;
        }

        var tableRange = worksheet.Range(1, 1, data.Count() + 1, settings.Columns.Count);
        tableRange.CreateTable();
    }

    private void SetTableHeader(IXLWorksheet worksheet)
    {
        if (!AreSettingsValidForGeneration())
        {
            return;
        }

        var columnIndex = 1;
        foreach (var column in settings.Columns)
        {
            var cell = worksheet.Cell(HeaderRow, columnIndex);

            cell.Value = column.HeaderText ?? GetMemberName(column.PropertyExpression) ?? "Unknown";
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(settings.Header?.Color ?? DefaultHeaderColor);
            columnIndex++;
        }
    }

    private void WriteData(IXLWorksheet worksheet)
    {
        if (!AreSettingsValidForGeneration())
        {
            return;
        }

        var columnIndex = 1;
        foreach (var column in settings.Columns)
        {
            WriteColumn(worksheet, column, columnIndex);
            columnIndex++;
        }
    }

    private void WriteColumn(IXLWorksheet worksheet, ColumnSettings<TData> column, int columnIndex)
    {
        if (column.Width is not null)
        {
            worksheet.Column(columnIndex).Width = column.Width.Value;
        }

        var rowIndex = HeaderRow + 1;
        var lambda = column.PropertyExpression?.Compile();
        var columnTypeName = GetTypeNameFromColumn(column);
        foreach (var item in data)
        {
            var value = lambda?.Invoke(item);
            var cell = worksheet.Cell(rowIndex, columnIndex);
            cell.Value = GetCellValue(value, columnTypeName);
            SetCellFormat(cell, columnTypeName);

            rowIndex++;
        }
    }

    private XLCellValue GetCellValue(object? value, string? valueTypeName)
    {
        if (value is null)
        {
            return Blank.Value;
        }

        if (value is Enum enumValue)
        {
            return GetEnumDisplayName(enumValue);
        }

        return valueTypeName switch
        {
            nameof(DateTimeOffset) => XLCellValue.FromObject(((DateTimeOffset)value).UtcDateTime, CultureInfo.InvariantCulture),
            nameof(DateOnly) => XLCellValue.FromObject(((DateOnly)value).ToDateTime(TimeOnly.MinValue), CultureInfo.InvariantCulture),
            nameof(Boolean) => GetBooleanValue(value),
            _ => XLCellValue.FromObject(value, CultureInfo.InvariantCulture),
        };
    }

    private static XLCellValue GetEnumDisplayName(Enum value)
    {
        var displayName = value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttribute<DisplayAttribute>()
            ?.GetName();

        return displayName ?? value.ToString();
    }

    private XLCellValue GetBooleanValue(object value)
    {
        if (value is not bool booleanValue)
        {
            return Blank.Value;
        }

        if (settings.Language is not null && settings.Language != DefaultLanguageCode)
        {
            return booleanValue ? "Ano" : "Ne";
        }

        return booleanValue ? "Yes" : "No";
    }

    private bool AreSettingsValidForGeneration()
        => settings.Columns.Count > 0;
}