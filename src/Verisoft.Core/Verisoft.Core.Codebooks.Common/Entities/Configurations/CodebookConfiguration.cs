using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Verisoft.Core.Codebooks.Common.Entities.Configurations;

internal sealed class CodebookConfiguration : CodebookBaseEntityTypeConfiguration<CodebookEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CodebookEntity> builder)
    {
    }
}
