using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.Data.EntityFramework.Test.Walkers.TestInfrastructure;

public class TestDbContext : EntityFrameworkDbContextBase
{
    private readonly string dbName;

    public TestDbContext(string dbName, IUserContext userContext, IWalkerFactory walkerFactory, TimeProvider timeProvider)
        : base(userContext, walkerFactory, timeProvider)
    {
        this.dbName = dbName;
    }

    public DbSet<ParentEntity> Parents { get; set; } = null!;

    public DbSet<CascadeChildEntity> CascadeChildren { get; set; } = null!;

    public DbSet<GrandChildEntity> GrandChildren { get; set; } = null!;

    public DbSet<RestrictChildEntity> RestrictChildren { get; set; } = null!;

    public DbSet<SetNullChildEntity> SetNullChildren { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(dbName);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CascadeChildEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.CascadeChildren)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GrandChildEntity>()
            .HasOne(e => e.CascadeChild)
            .WithMany(e => e.GrandChildren)
            .HasForeignKey(e => e.CascadeChildId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RestrictChildEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.RestrictChildren)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SetNullChildEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.SetNullChildren)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
