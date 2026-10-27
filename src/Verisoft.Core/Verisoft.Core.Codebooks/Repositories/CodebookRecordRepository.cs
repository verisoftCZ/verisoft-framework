using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Repositories;
using EFStatic = Microsoft.EntityFrameworkCore.EF;

namespace Verisoft.Core.Codebooks.Repositories;

public class CodebookRecordRepository<TContext, TCodebookRecordType>(TContext unitOfWork, IUserContext userContext)
    : EntityFrameworkRepositoryBase<CodebookRecordEntity<TCodebookRecordType>, int>(unitOfWork), ICodebookRecordRepository<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
    where TContext : IUnitOfWork
{
    public virtual async Task<CodebookRecordWithTranslation<TCodebookRecordType>[]> GetPagedCodebookRecordsByTranslationAsync(int offset, int limit, string languageCode, string? translationFilter, bool isAscending = true, string[]? stringValue = null, string[]? exclude = null)
    {
        var query = GetCodebookRecordsByTranslationQuery(languageCode, translationFilter, stringValue, exclude);
        query = isAscending ? query.OrderBy(x => x.Translation) : query.OrderByDescending(x => x.Translation);

        return await query.Skip(offset).Take(limit).ToArrayAsync();
    }

    public virtual async Task<CodebookRecordEntity<TCodebookRecordType>?> GetStringValueDetailAsync(string stringValue) => await GetQueryable().FirstOrDefaultAsync(x => x.StringValue == stringValue);

    public virtual async Task<int> GetCodebookRecordsByTranslationCountAsync(string languageCode, string? translationFilter, string[]? stringValue = null, string[]? exclude = null) => await GetCodebookRecordsByTranslationQuery(languageCode, translationFilter, stringValue, exclude).CountAsync();

    public void HardDeleteCodebookRecord(CodebookRecordEntity<TCodebookRecordType> record)
    {
        GetDbSet().Remove(record);
    }

    protected override DbSet<CodebookRecordEntity<TCodebookRecordType>> GetDbSet() => ((DbContext)UnitOfWork).Set<CodebookRecordEntity<TCodebookRecordType>>();

    protected override IQueryable<CodebookRecordEntity<TCodebookRecordType>> GetQueryable() => base.GetQueryable()
        .Include(x => x.Translations);

    protected virtual IQueryable<CodebookRecordWithTranslation<TCodebookRecordType>> GetCodebookRecordsByTranslationQuery(string languageCode, string? translationFilter, string[]? stringValue = null, string[]? exclude = null)
    {
        var query = GetQueryable()
            .Where(x => x.ValidFrom <= DateOnly.FromDateTime(DateTime.Today) && (x.ValidTo == null || x.ValidTo >= DateOnly.FromDateTime(DateTime.Today))
                && (exclude == null || !exclude.Any() || !exclude.Contains(x.StringValue))
                && (stringValue == null || !stringValue.Any() || stringValue.Contains(x.StringValue)))
            .Select(x => new CodebookRecordWithTranslation<TCodebookRecordType>
            {
                Record = x,
                Translation = x.Translations
                       .Where(t => t.LanguageCode == languageCode)
                       .Select(t => t.Value)
                       .FirstOrDefault()
                    ??
                    x.Translations
                        .Where(t => t.LanguageCode == LanguageConstants.DefaultLanguageCode)
                        .Select(t => t.Value)
                        .First(),
            });

        if (!string.IsNullOrWhiteSpace(translationFilter))
        {
            string upperTranslationFilter = translationFilter.ToUpper();
            string pattern = string.Concat("%", upperTranslationFilter, "%");
            Expression<Func<CodebookRecordWithTranslation<TCodebookRecordType>, bool>> predicate = x => EFStatic.Functions.Like(x.Translation.ToUpper(), pattern);
            query = query.Where(predicate);
        }

        return query;
    }
}