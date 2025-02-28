using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.CodebookApi.Core.Constants;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class CodebookRecordRepository<TContext, TCodebookRecordEntity>(TContext unitOfWork, IUserContext userContext, ILogger<CodebookRecordRepository<TContext, TCodebookRecordEntity>> logger)
    : EntityFrameworkRepositoryBase<TCodebookRecordEntity, int>(unitOfWork, userContext, logger), ICodebookRecordRepository<TCodebookRecordEntity>
    where TCodebookRecordEntity : class, ICodebookRecordEntity, new()
    where TContext : IUnitOfWork
{
    private readonly string tableName = typeof(TCodebookRecordEntity).Name.Replace("Entity", string.Empty);

    public async Task<List<TCodebookRecordEntity>> GetCodebookRecordsAsync(string? stringValue = null, string? searchTerm = null)
    {
        var tenantValuesDbSet = ((DbContext)UnitOfWork).Set<TenantValueEntity>();

        var tempResult = await GetQueryable()
            .SelectMany(
            record => tenantValuesDbSet,
            (record, tenantValue) => new { record, tenantValue })
            .Where(x => x.record.Id == x.tenantValue.CodebookRecordId
                        && (string.IsNullOrWhiteSpace(searchTerm) || x.record.StringValue.Contains(searchTerm) || (x.record.Description ?? string.Empty).Contains(searchTerm))
                        && (string.IsNullOrWhiteSpace(stringValue) || x.record.StringValue == stringValue)
                        && x.tenantValue.TableName == tableName)
            .GroupBy(x => x.record)
            .Select(g => new
            {
                Record = g.Key,
                TenantValues = g.Select(x => x.tenantValue),
            })
            .ToListAsync();

        return tempResult.Select(x =>
        {
            x.Record.TenantValues = x.TenantValues.ToHashSet();
            return x.Record;
        }).ToList();
    }

    public async Task<List<TCodebookRecordEntity>> GetCodebookRecordsByTranslationAsync(string? translationFilter, int tenantId, int languageId)
    {
        var recordsWithMatchingTranslations = await GetRecordsWithMatchingTranslation(translationFilter, tenantId, languageId);
        var recordIds = recordsWithMatchingTranslations.Select(x => x.Record.Id);
        var allTranslationsForLanguageId = await GetAllTranslationsForLanguage(languageId, recordIds);

        var resultList = new List<TCodebookRecordEntity>();

        foreach (var (record, tenantValue, matchingTranslations) in recordsWithMatchingTranslations)
        {
            SetTranslationToRecord(languageId, allTranslationsForLanguageId, matchingTranslations, record);

            if (record.Translations != null)
            {
                record.TenantValues = [tenantValue];
                resultList.Add(record);
            }
        }

        return resultList;
    }

    protected override DbSet<TCodebookRecordEntity> GetDbSet()
    {
        return ((DbContext)UnitOfWork).Set<TCodebookRecordEntity>();
    }

    protected override IQueryable<TCodebookRecordEntity> GetQueryable()
    {
        return base.GetQueryable().Where(x => !x.IsDeleted);
    }

    private static void SetTranslationToRecord(int languageId, List<(TCodebookRecordEntity Record, TranslationEntity Translation)> allTranslationsForLanguageId, IEnumerable<TranslationEntity> matchingTranslations, TCodebookRecordEntity record)
    {
        var availableTranslation = matchingTranslations.FirstOrDefault(t => t.LanguageId == languageId)
                                               ?? matchingTranslations.First(t => t.LanguageId == LanguageConstants.DefaultLanguageId);
        var translationForRequestedLanguageIdExists = allTranslationsForLanguageId.Any(t => t.Record.Id == record.Id);
        record.Translations = translationForRequestedLanguageIdExists && availableTranslation.LanguageId != languageId ? null : [availableTranslation];
    }

    private async Task<IEnumerable<(TCodebookRecordEntity Record, TenantValueEntity TenantValue, IEnumerable<TranslationEntity> Translations)>> GetRecordsWithMatchingTranslation(string? translationFilter, int tenantId, int languageId)
    {
        return (await (from record in GetQueryable()
                       join translation in ((DbContext)UnitOfWork).Set<TranslationEntity>()
                       on new { record.StringValue, TableName = tableName }
                       equals new { StringValue = translation.TableStringValue, translation.TableName }
                       into translationJoin
                       from translation in translationJoin.DefaultIfEmpty()
                       join tenantValue in ((DbContext)UnitOfWork).Set<TenantValueEntity>()
                       on new { TableName = tableName, CodebookRecordId = record.Id }
                       equals new { tenantValue.TableName, tenantValue.CodebookRecordId }
                       where tenantValue.TenantId == tenantId &&
                            !tenantValue.IsForbidden &&
                             translation != null &&
                             (string.IsNullOrWhiteSpace(translationFilter) || translation.Value.Contains(translationFilter)) &&
                             (translation.LanguageId == languageId || translation.LanguageId == LanguageConstants.DefaultLanguageId)
                       select new { Record = record, Translation = translation, TenantValue = tenantValue })
                                                      .ToListAsync())
                                                      .GroupBy(x => new { x.Record, x.TenantValue })
                                                      .Select(x => (x.Key.Record, x.Key.TenantValue, Translations: x.Select(g => g.Translation)));
    }

    private async Task<List<(TCodebookRecordEntity CodebookRecordEntity, TranslationEntity TranslationEntity)>> GetAllTranslationsForLanguage(int languageId, IEnumerable<int> recordIds)
    {
        return (await (from record in GetQueryable()
                       join translation in ((DbContext)UnitOfWork).Set<TranslationEntity>()
                       on new { record.StringValue, TableName = tableName }
                       equals new { StringValue = translation.TableStringValue, translation.TableName }
                       into translationJoin
                       from translation in translationJoin.DefaultIfEmpty()
                       where recordIds.Contains(record.Id) && translation.LanguageId == languageId
                       select new Tuple<TCodebookRecordEntity, TranslationEntity>(record, translation))
                                                      .ToListAsync()).Select(x => (x.Item1, x.Item2)).ToList();
    }
}
