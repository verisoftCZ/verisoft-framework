using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Common.Models.Bulk;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Services;

public class BulkStringValueService(IServiceProvider serviceProvider, ILanguageRepository languageRepository, ICodebookRegistry codebookRegistry) : IBulkStringValueService
{
    private static readonly string[] AllowedExtensions = [".xlsx", ".xls"];

    private static readonly List<string> IgnoreColumns =
    [
        "TableName",
        "StringValue",
        "Description",
    ];

    private BulkImportResult importResult = new();

    public async Task<BulkImportResult> ImportStringValueDetailFromExcelFileAsync(IFormFile file)
    {
        importResult = ValidateFile(file);

        if (importResult.Errors.Count > 0)
        {
            return importResult;
        }

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        foreach (var worksheet in workbook.Worksheets)
        {
            var worksheetHeaderDictionary = GetWorksheetHeaderDictionary(worksheet);

            var rowTableGroups = GetWorksheetRowsGroupedByTableName(worksheet.Worksheet, worksheetHeaderDictionary);

            if (worksheetHeaderDictionary.Count > 0)
            {
                var methodInfo = typeof(BulkStringValueService).GetMethod(nameof(ProcessTableAsync), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
                if (methodInfo is null)
                {
                    importResult.Errors.Add("Method ProcessTableAsync is not found.");
                    continue;
                }

                foreach (var table in rowTableGroups)
                {
                    var codebookRecordType = GetCodebookRecordType(table.Key);
                    if (codebookRecordType is null)
                    {
                        importResult.Errors.Add($"CodebookRecordType '{table.Key}' does not exist.");
                        continue;
                    }

                    var genericMethod = methodInfo.MakeGenericMethod(codebookRecordType);
                    await (Task)genericMethod.Invoke(this, [table, worksheetHeaderDictionary])!;
                }
            }
        }

        return importResult;
    }

    private Type? GetCodebookRecordType(string tableName)
        => codebookRegistry
            .GetRegisteredCodebooks()
            .FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

    private static Dictionary<string, int> GetWorksheetHeaderDictionary(IXLWorksheet worksheet)
    {
        return worksheet.Row(1)
              .CellsUsed()
              .Select((cell, index) => new { Header = cell.GetString(), Index = index + 1 })
              .ToDictionary(h => h.Header, h => h.Index);
    }

    private static void MapAdditionalProperties<TCodebookRecordType>(Dictionary<string, string> additionalValueDictionary, TCodebookRecordType additionalPropertiesInstance)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var additionalPropertiesType = additionalPropertiesInstance.GetType();
        foreach (var additionalValueKeyValuePair in additionalValueDictionary)
        {
            var property = additionalPropertiesType.GetProperty(additionalValueKeyValuePair.Key);
            if (property is not null && property.CanWrite)
            {
                var targetValue = Convert.ChangeType(additionalValueKeyValuePair.Value, property.PropertyType);
                property.SetValue(additionalPropertiesInstance, targetValue);
            }
        }
    }

    private static Dictionary<string, string> GetTranslationDictionary(Dictionary<string, int> headers, IXLRow row)
    {
        return headers.Keys
            .Where(h => h.StartsWith(BulkCodebookImportHeaderNameConstants.Translation, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(headerValue => headerValue, headerValue => row.Cell(headers[headerValue]).GetString());
    }

    private static Dictionary<string, string> GetAdditionalValuesDictionary(Dictionary<string, int> headers, IXLRow row)
    {
        return headers.Keys
            .Where(h => !IgnoreColumns.Contains(h) && !h.StartsWith(BulkCodebookImportHeaderNameConstants.Translation, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(headerValue => headerValue, headerValue => row.Cell(headers[headerValue]).GetString());
    }

    private static BulkImportResult ValidateFile(IFormFile file)
    {
        if (file?.Length is null or 0)
        {
            return new("No file found.");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!AllowedExtensions.Contains(fileExtension))
        {
            return new($"Unsupported file format. Allowed file extensions: {string.Join(", ", AllowedExtensions)}");
        }

        return new();
    }

    private static StringValueImportRow GetStringValueDetailsFromGroupedRows(IXLRow row, Dictionary<string, int> headers)
    {
        return new StringValueImportRow
        {
            StringValue = row.Cell(headers[BulkCodebookImportHeaderNameConstants.StringValue]).GetString(),
            ValidFrom = row.Cell(headers[BulkCodebookImportHeaderNameConstants.ValidFrom]).GetString(),
            ValidTo = row.Cell(headers[BulkCodebookImportHeaderNameConstants.ValidTo]).GetString(),
            Description = row.Cell(headers[BulkCodebookImportHeaderNameConstants.Description]).GetString(),
            TranslationDictionary = GetTranslationDictionary(headers, row),
            AdditionalPropertiesDictionary = GetAdditionalValuesDictionary(headers, row),
        };
    }

    private static void ValidateStringValueInput(BulkImportStringValueResult stringValueImportResult, string? defaultTranslation, string? validFrom, string? validTo)
    {
        if (string.IsNullOrWhiteSpace(defaultTranslation))
        {
            stringValueImportResult.Errors.Add("Default translation was not specified");
        }

        if (string.IsNullOrWhiteSpace(validFrom))
        {
            stringValueImportResult.Errors.Add("ValidFrom was not specified");
        }
        else if (!DateOnly.TryParseExact(validFrom, "yyyy-MM-dd", out _))
        {
            stringValueImportResult.Errors.Add("ValidFrom is not in required format: 'yyyy-MM-dd'");
        }

        if (!string.IsNullOrWhiteSpace(validTo) && !DateOnly.TryParseExact(validTo, "yyyy-MM-dd", out _))
        {
            stringValueImportResult.Errors.Add("ValidTo is not in required format: 'yyyy-MM-dd' or empty");
        }
    }

    private static StringValueDetailCreateModel<TCodebookRecordType> GetStringValueDetail<TCodebookRecordType>(string stringValue, StringValueImportRow importRow, string defaultTranslation, string validFrom, string? validTo)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var additionalProperties = new TCodebookRecordType();
        MapAdditionalProperties(importRow.AdditionalPropertiesDictionary, additionalProperties);
        return new StringValueDetailCreateModel<TCodebookRecordType>
        {
            DefaultTranslation = defaultTranslation,
            StringValue = stringValue,
            AdditionalProperties = additionalProperties,
            ValidFrom = DateOnly.ParseExact(validFrom, "yyyy-MM-dd"),
            ValidTo = string.IsNullOrWhiteSpace(validTo) ? null : DateOnly.ParseExact(validTo, "yyyy-MM-dd"),
        };
    }

    private static StringValueDetailEditModel<TCodebookRecordType> GetStringValueDetailEditModel<TCodebookRecordType>(StringValueImportRow importRow, string defaultTranslation, string validFrom, string? validTo)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var additionalProperties = new TCodebookRecordType();
        MapAdditionalProperties(importRow.AdditionalPropertiesDictionary, additionalProperties);
        return new StringValueDetailEditModel<TCodebookRecordType>
        {
            DefaultTranslation = defaultTranslation,
            AdditionalProperties = additionalProperties,
            ValidFrom = DateOnly.ParseExact(validFrom, "yyyy-MM-dd"),
            ValidTo = string.IsNullOrWhiteSpace(validTo) ? null : DateOnly.ParseExact(validTo, "yyyy-MM-dd"),
            Description = string.IsNullOrWhiteSpace(importRow.Description) ? null : importRow.Description,
        };
    }

    private List<IGrouping<string, IXLRow>> GetWorksheetRowsGroupedByTableName(IXLWorksheet worksheet, Dictionary<string, int> worksheetHeaderDictionary)
    {
        var groupedRows = worksheet.RowsUsed()
            .Skip(1)
            .GroupBy(row =>
                row.Cell(worksheetHeaderDictionary[BulkCodebookImportHeaderNameConstants.TableName])?.GetString() ?? string.Empty)
            .ToList();

        var emptyTableGroup = groupedRows.FirstOrDefault(g => string.IsNullOrWhiteSpace(g.Key));
        if (emptyTableGroup is not null)
        {
            importResult.Errors.Add($"Following string values have empty 'TableName': {string.Join(", ", emptyTableGroup.Select(x => x.Cell(worksheetHeaderDictionary[BulkCodebookImportHeaderNameConstants.StringValue])?.GetString() ?? string.Empty))}");
        }

        return groupedRows.Where(g => !string.IsNullOrWhiteSpace(g.Key)).ToList();
    }

    private async Task ProcessTableAsync<TCodebookRecordType>(IGrouping<string, IXLRow> tableRows, Dictionary<string, int> worksheetHeaderDictionary)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var tableName = tableRows.Key;
        var service = serviceProvider.GetService<IStringValueService<TCodebookRecordType>>();

        var codebookImportResult = new BulkImportCodebookResult(tableName);
        importResult.Codebooks.Add(codebookImportResult);

        if (service is null)
        {
            codebookImportResult.Error = "Service is not available.";
            return;
        }

        if (tableRows.Any(r => string.IsNullOrWhiteSpace(r.Cell(worksheetHeaderDictionary[BulkCodebookImportHeaderNameConstants.StringValue])?.GetString())))
        {
            codebookImportResult.Error = "Some rows have empty 'StringValue'.";
        }

        var allExistingStringValues = await service.GetStringValueDetailListAsync(new());
        var existingStringValuesSet = allExistingStringValues.Data.Data
            .Select(sv => sv.StringValue)
            .ToHashSet();

        var importRows = tableRows
            .Where(r => !string.IsNullOrWhiteSpace(r.Cell(worksheetHeaderDictionary[BulkCodebookImportHeaderNameConstants.StringValue])?.GetString()))
            .Select(row => GetStringValueDetailsFromGroupedRows(row, worksheetHeaderDictionary)).ToList();

        var excelStringValues = importRows.Select(r => r.StringValue).ToHashSet();

        var stringValuesToDelete = existingStringValuesSet.Except(excelStringValues).ToHashSet();

        foreach (var stringValueToDelete in stringValuesToDelete)
        {
            var deleteResult = await service.RemoveStringValueAsync(stringValueToDelete);

            if (!deleteResult.IsSuccess)
            {
                var deletionResult = new BulkImportStringValueResult(stringValueToDelete);
                deletionResult.Errors.AddRange(deleteResult.Errors.Select(e => e.Message));
                codebookImportResult.StringValues.Add(deletionResult);
            }
        }

        foreach (var importRow in importRows.Where(x => !stringValuesToDelete.Contains(x.StringValue)))
        {
            var stringValue = importRow.StringValue;
            var stringValueImportResult = new BulkImportStringValueResult(stringValue);
            codebookImportResult.StringValues.Add(stringValueImportResult);

            var defaultTranslation = importRow.TranslationDictionary.Where(t => string.Equals(t.Key, BulkCodebookImportHeaderNameConstants.DefaultTranslation, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.Value)).Value;

            ValidateStringValueInput(stringValueImportResult, defaultTranslation, importRow.ValidFrom, importRow.ValidTo);

            if (stringValueImportResult.Errors.Count > 0)
            {
                continue;
            }

            var isExistingStringValue = existingStringValuesSet.Contains(stringValue);

            if (isExistingStringValue)
            {
                var existingDetail = allExistingStringValues.Data.Data.First(sv => sv.StringValue.Equals(stringValue, StringComparison.OrdinalIgnoreCase));
                var stringValueDetailEditModel = GetStringValueDetailEditModel<TCodebookRecordType>(importRow, defaultTranslation, importRow.ValidFrom!, importRow.ValidTo);
                if (!IsChanged(existingDetail, stringValueDetailEditModel))
                {
                    continue;
                }

                var validatedResult = await service.ValidateAsync(stringValue, stringValueDetailEditModel, false);

                if (validatedResult.IsSuccess)
                {
                    await service.UpdateStringValueAsync(stringValue, stringValueDetailEditModel);

                    var translationsToSave = await GetTranslationsToSaveAsync(importRow.TranslationDictionary, stringValueImportResult);

                    foreach (var translation in translationsToSave)
                    {
                        await service.UpsertTranslationAsync(stringValue, translation.LanguageCode, translation.Value);
                    }
                }
                else
                {
                    foreach (var error in validatedResult.Errors)
                    {
                        stringValueImportResult.Errors.Add($"{error.Message}");
                    }
                }
            }
            else
            {
                var stringValueDetailCreateModel = GetStringValueDetail<TCodebookRecordType>(stringValue, importRow, defaultTranslation, importRow.ValidFrom!, importRow.ValidTo);

                var validatedResult = await service.ValidateAsync(stringValue, stringValueDetailCreateModel, true);

                if (validatedResult.IsSuccess)
                {
                    await service.AddStringValueAsync(stringValueDetailCreateModel);

                    var translationsToSave = await GetTranslationsToSaveAsync(importRow.TranslationDictionary, stringValueImportResult);

                    foreach (var translation in translationsToSave)
                    {
                        await service.UpsertTranslationAsync(stringValue, translation.LanguageCode, translation.Value);
                    }
                }
                else
                {
                    foreach (var error in validatedResult.Errors)
                    {
                        stringValueImportResult.Errors.Add($"{error.Message}");
                    }
                }
            }
        }
    }

    private bool IsChanged<TCodebookRecordType>(StringValueDetail<TCodebookRecordType> existingDetail, StringValueDetailEditModel<TCodebookRecordType> stringValueDetailEditModel)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var modelToCompare = new StringValueDetailEditModel<TCodebookRecordType>
        {
            DefaultTranslation = existingDetail.DefaultTranslation,
            AdditionalProperties = existingDetail.AdditionalProperties,
            ValidFrom = existingDetail.ValidFrom,
            ValidTo = existingDetail.ValidTo,
            Description = existingDetail.Description,
        };

        return modelToCompare.DefaultTranslation != stringValueDetailEditModel.DefaultTranslation ||
               modelToCompare.ValidFrom != stringValueDetailEditModel.ValidFrom ||
               modelToCompare.ValidTo != stringValueDetailEditModel.ValidTo ||
               modelToCompare.Description != stringValueDetailEditModel.Description ||
               IsAdditionalPropertiesChanged(modelToCompare, stringValueDetailEditModel);
    }

    private bool IsAdditionalPropertiesChanged<TCodebookRecordType>(StringValueDetailEditModel<TCodebookRecordType> modelToCompare, StringValueDetailEditModel<TCodebookRecordType> stringValueDetailEditModel)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var additionalPropertiesType = typeof(TCodebookRecordType);
        var properties = additionalPropertiesType.GetProperties().Where(p => p.CanRead && p.CanWrite);
        foreach (var property in properties)
        {
            var value1 = property.GetValue(modelToCompare.AdditionalProperties);
            var value2 = property.GetValue(stringValueDetailEditModel.AdditionalProperties);
            if (value1 is null && value2 is null)
            {
                continue;
            }

            if (value1 is null || value2 is null)
            {
                return true;
            }

            if (!value1.Equals(value2))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<Translation[]> GetTranslationsToSaveAsync(IEnumerable<KeyValuePair<string, string>> translations, BulkImportStringValueResult stringValueImportResult)
    {
        var translationDictionary = new Dictionary<string, string>();

        foreach (var translation in translations.Where(t => !string.IsNullOrWhiteSpace(t.Value)))
        {
            var languageCode = translation.Key.Split(':')[1].ToLower();
            translationDictionary.TryAdd(languageCode, translation.Value);
        }

        var languages = await languageRepository.GetAllAsync();

        var unknownLanguageCodes = translationDictionary.Keys.Except(languages.Select(l => l.Code.ToLower())).ToList();

        if (unknownLanguageCodes.Count != 0)
        {
            stringValueImportResult.Errors.Add($"Failed to add some translations - unknown language codes: {string.Join(", ", unknownLanguageCodes)}");
        }

        return languages
            .Where(l => translationDictionary.ContainsKey(l.Code.ToLower()))
            .Select(l => new Translation
            {
                LanguageCode = l.Code,
                Value = translationDictionary[l.Code.ToLower()],
            })
            .ToArray();
    }
}