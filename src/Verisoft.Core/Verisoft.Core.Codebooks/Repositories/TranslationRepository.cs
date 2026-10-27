using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.Core.Codebooks.Repositories;

public class TranslationRepository<TContext, TCodebookRecordType>(TContext unitOfWork) : EntityFrameworkRepositoryBase<CodebookRecordTranslationEntity<TCodebookRecordType>, int>(unitOfWork), ITranslationRepository<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
    where TContext : IUnitOfWork
{
    public async Task<CodebookRecordTranslationEntity<TCodebookRecordType>?> GetTranslationByRecordIdAndLanguageCodeAsync(int codebookRecordId, string languageCode)
    {
        return await GetQueryable().FirstOrDefaultAsync(x => x.CodebookRecordId == codebookRecordId && x.LanguageCode == languageCode);
    }

    public async Task<CodebookRecordTranslationEntity<TCodebookRecordType>[]> GetTranslationsByRecordIdAsync(int codebookRecordId)
    {
        return await GetQueryable().Where(x => x.CodebookRecordId == codebookRecordId).ToArrayAsync();
    }

    protected override DbSet<CodebookRecordTranslationEntity<TCodebookRecordType>> GetDbSet()
    {
        return ((DbContext)UnitOfWork).Set<CodebookRecordTranslationEntity<TCodebookRecordType>>();
    }
}
