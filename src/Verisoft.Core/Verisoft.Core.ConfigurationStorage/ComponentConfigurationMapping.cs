using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Verisoft.Core.ConfigurationStorage;

public sealed class ComponentConfigurationMapping : IEntityTypeConfiguration<ComponentConfiguration>
{
    public void Configure(EntityTypeBuilder<ComponentConfiguration> builder)
    {
        builder.ToTable("ComponentConfigurations", table =>
        {
            table.HasCheckConstraint("CK_ComponentConfigurations_Owner", "([Level] = 1 AND [UserId] IS NULL) OR ([Level] = 2 AND [UserId] IS NOT NULL)");
            table.HasCheckConstraint("CK_ComponentConfigurations_SchemaVersion", "[SchemaVersion] > 0");
        });
        builder.HasKey(configuration => configuration.Id);
        builder.Property(configuration => configuration.Context).HasMaxLength(100).IsRequired();
        builder.Property(configuration => configuration.ComponentId).HasMaxLength(200).IsRequired();
        builder.Property(configuration => configuration.ConfigurationType).HasMaxLength(100).IsRequired();
        builder.Property(configuration => configuration.UserId).HasMaxLength(200);
        builder.Property(configuration => configuration.Data).IsRequired();
        builder.HasIndex(configuration => new { configuration.Context, configuration.ComponentId, configuration.ConfigurationType })
            .IsUnique().HasFilter("[Level] = 1");
        builder.HasIndex(configuration => new { configuration.Context, configuration.ComponentId, configuration.ConfigurationType, configuration.UserId })
            .IsUnique().HasFilter("[Level] = 2");
    }
}