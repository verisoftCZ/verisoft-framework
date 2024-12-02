using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Extensions;

namespace Verisoft.Core.Authentication.MicrosoftIdentity.Data;

public class IdentityEntityFrameworkDbContext<TUser>(IDatabaseConfig databaseConfig)
    : IdentityDbContext<TUser>, IUnitOfWork, IMigrable
    where TUser : IdentityUser
{
    private readonly IDatabaseConfig databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));

    public void Commit()
    {
        this.SaveChanges();
    }

    public void Migrate()
    {
        this.Database.Migrate();
    }

    public void Rollback()
    {
        this.RollbackChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
            databaseConfig.ConnectionString,
            x => x.MigrationsHistoryTable("__IdentityMigrationsHistory"));
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Constants.VerisoftIdentitySchemaName);

        base.OnModelCreating(builder);
    }
}
