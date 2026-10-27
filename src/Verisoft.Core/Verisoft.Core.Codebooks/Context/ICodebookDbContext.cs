using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.Codebooks.Context;

public interface ICodebookDbContext : IUnitOfWork, IMigrable
{
    DbSet<LanguageEntity> Language { get; set; }

    DbSet<CodebookEntity> Codebook { get; set; }
}