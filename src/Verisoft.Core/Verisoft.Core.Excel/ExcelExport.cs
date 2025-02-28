using ClosedXML.Excel;
using System.Linq.Expressions;
using System.Reflection;
using Verisoft.Core.Excel.Builders;
using Verisoft.Core.Excel.Helpers;
using Verisoft.Core.Excel.Settings;

namespace Verisoft.Core.Excel;

public class ExcelExport<TData>(IEnumerable<TData> data)
{
    private const int HeaderRow = 1;
    private const string DefaultHeaderColor = "#FF950E";
    private const string DefaultSheetName = "Sheet1";

    private readonly ExportSettings<TData> settings = new();

    public ExcelExport<TData> Header(Action<HeaderBuilder> configuration)
    {
        var builder = new HeaderBuilder();
        configuration.Invoke(builder);
        settings.Header = builder.Build();

        return this;
    }

    public ExcelExport<TData> SheetName(string name)
    {
        settings.SheetName = name;
        return this;
    }

    public ExcelExport<TData> Columns(Action<ColumnsBuilder<TData>> configuration)
    {
        var builder = new ColumnsBuilder<TData>();
        configuration.Invoke(builder);
        settings.Columns = builder.Build();

        return this;
    }

    public void Export(Stream stream)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(settings.SheetName ?? DefaultSheetName);

        if (settings.Columns is null)
        {
            settings.Columns = GenerateColumns();
        }

        SetAsTable(worksheet);
        SetTableHeader(worksheet);
        WriteData(worksheet);

        workbook.SaveAs(stream);
    }

    private List<ColumnSettings<TData>> GenerateColumns()
    {
        List<ColumnSettings<TData>> columns = new();

        Type type = typeof(TData);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            ParameterExpression parameter = Expression.Parameter(type, "x");
            MemberExpression propertyAccess = Expression.Property(parameter, property);

            UnaryExpression convertedExpression = Expression.Convert(propertyAccess, typeof(object));

            var lambda = Expression.Lambda<Func<TData, object>>(convertedExpression, parameter);

            columns.Add(new ColumnSettings<TData>() { PropertyExpression = lambda });
        }

        return columns;
    }

    private void SetAsTable(IXLWorksheet worksheet)
    {
        if (settings.Columns is null)
        {
            return;
        }

        var tableRange = worksheet.Range(1, 1, data.Count() + 1, settings.Columns.Count);
        tableRange.CreateTable();
    }

    private void WriteData(IXLWorksheet worksheet)
    {
        if (settings.Columns is null)
        {
            return;
        }

        var columnIndex = 1;
        foreach (var column in settings.Columns)
        {
            var lambda = column.PropertyExpression.Compile();
            if (column.Width is not null)
            {
                worksheet.Column(columnIndex).Width = column.Width.Value;
            }

            var rowIndex = HeaderRow + 1;

            foreach (var item in data)
            {
                var value = lambda(item);
                worksheet.Cell(rowIndex, columnIndex).Value = value?.ToString();
                rowIndex++;
            }

            columnIndex++;
        }
    }

    private void SetTableHeader(IXLWorksheet worksheet)
    {
        if (settings.Columns is null)
        {
            return;
        }

        int columnIndex = 1;

        foreach (var column in settings.Columns)
        {
            var cell = worksheet.Cell(HeaderRow, columnIndex);

            cell.Value = column.HeaderText ?? ExcelExportHelper.GetMemberName(column.PropertyExpression);
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(settings.Header?.Color ?? DefaultHeaderColor);
            columnIndex++;
        }
    }
}