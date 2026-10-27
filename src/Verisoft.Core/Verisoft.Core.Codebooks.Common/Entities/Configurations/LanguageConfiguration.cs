using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Verisoft.Core.Codebooks.Common.Entities.Configurations;

internal sealed class LanguageConfiguration : CodebookBaseEntityTypeConfiguration<LanguageEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<LanguageEntity> builder)
    {
        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(25);

        builder.HasData(new LanguageEntity
        {
            Id = 1,
            Name = "Default",
            Code = "Default",
            IsDefault = true,
            CreatedBy = "System",
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
            IsDeleted = false,
        });
    }
}