using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.CodebookApi.Core.Entities;

namespace Verisoft.CodebookApi.Database.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.HasOne(e => e.Language)
            .WithMany()
            .HasForeignKey(e => e.LanguageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}