using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework.Configuration;

public class EntityAuditDetailConfiguration : IEntityTypeConfiguration<EntityAuditDetailEntity>
{
    public void Configure(EntityTypeBuilder<EntityAuditDetailEntity> builder)
    {
        builder.ToTable("EntityAuditDetail", "audit");
        builder.HasOne(x => x.EntityAudit)
            .WithMany(x => x.EntityAuditDetails)
            .HasForeignKey(x => x.EntityAuditId);

        builder.Property(x => x.ChangedAttribute)
            .HasMaxLength(255);

        builder.HasIndex(x => x.ChangedAttribute);
        builder.HasIndex(x => x.EntityAuditId);
    }
}
