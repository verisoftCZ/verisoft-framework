using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework.Configuration;

public class EntityAuditConfiguration : IEntityTypeConfiguration<EntityAuditEntity>
{
    public void Configure(EntityTypeBuilder<EntityAuditEntity> builder)
    {
        builder.ToTable("EntityAudit", "audit");
        builder.HasMany(x => x.EntityAuditDetails)
            .WithOne(x => x.EntityAudit)
            .HasForeignKey(x => x.EntityAuditId);

        builder.Property(x => x.Schema)
            .HasMaxLength(255);
        builder.Property(x => x.Table)
            .HasMaxLength(255);
        builder.Property(x => x.EntityId)
            .HasMaxLength(255);
        builder.Property(x => x.ChangedBy)
            .HasMaxLength(255);

        builder.HasIndex(x => x.Table);
        builder.HasIndex(x => x.ChangedBy);
        builder.HasIndex(x => x.EntityId);
    }
}
