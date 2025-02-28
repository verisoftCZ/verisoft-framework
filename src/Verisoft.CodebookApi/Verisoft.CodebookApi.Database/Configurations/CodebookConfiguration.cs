using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.CodebookApi.Core.Entities;

namespace Verisoft.CodebookApi.Database.Configurations;

public class CodebookConfiguration : IEntityTypeConfiguration<CodebookEntity>
{
    public void Configure(EntityTypeBuilder<CodebookEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
