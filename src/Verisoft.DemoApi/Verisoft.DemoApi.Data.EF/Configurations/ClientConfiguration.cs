using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Data.EF.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasMany(client => client.Subsidiaries)
               .WithOne(client => client.ParentClient)
               .HasForeignKey(client => client.ParentClientId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
