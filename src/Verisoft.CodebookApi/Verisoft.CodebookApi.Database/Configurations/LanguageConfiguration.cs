using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.CodebookApi.Core.Entities;

namespace Verisoft.CodebookApi.Database.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<LanguageEntity>
{
    public void Configure(EntityTypeBuilder<LanguageEntity> builder)
    {
        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(25);

        builder.HasQueryFilter(r => !r.IsDeleted);

        builder.HasData(new LanguageEntity
        {
            Id = 1,
            Name = "Default",
            Code = "Default",
            IsDefault = true,
            CreatedBy = "System",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
        });
    }
}