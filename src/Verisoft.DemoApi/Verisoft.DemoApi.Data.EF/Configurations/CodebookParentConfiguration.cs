using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Codebooks.Common.Extensions;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Data.EF.Configurations;

internal sealed class CodebookParentConfiguration : IEntityTypeConfiguration<CodebookParentEntity>
{
    public void Configure(EntityTypeBuilder<CodebookParentEntity> builder)
    {
        builder.ConfigureCodebookRecord(x => x.Accessibility);

        builder
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId);

        builder.HasIndex(x => new { x.ClientId, x.Code })
            .IsUnique(true)
            .HasFilter($"{nameof(CodebookParentEntity.IsActive)} = 1");
    }
}