using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Reflection;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Atributes;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.Core.Data.EntityFramework.Extensions;

namespace Verisoft.Core.Data.EntityFramework;

public abstract class EntityFrameworkDbContextBase : DbContext, IUnitOfWork, IMigrable
{
    protected EntityFrameworkDbContextBase()
        : base()
    {
    }

    protected EntityFrameworkDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<EntityAuditEntity> EntityAudit { get; set; }

    public DbSet<EntityAuditDetailEntity> EntityAuditDetail { get; set; }

    public string DefaultSchema { get; protected set; } = "dbo";

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EntityAuditConfiguration());
        modelBuilder.ApplyConfiguration(new EntityAuditDetailConfiguration());
        AddVersionPropertyForOptimisticConcurency(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        RegisterInterceptorsByAttribute(optionsBuilder);
    }

    private static void RegisterInterceptorsByAttribute(DbContextOptionsBuilder optionsBuilder)
    {
        var interceptors = AppDomain.CurrentDomain.GetAssemblies()
       .Where(assembly => assembly.FullName.StartsWith("Verisoft"))
       .SelectMany(assembly => assembly.GetTypes())
       .Where(x => x.IsClass && x.IsPublic)
       .Where(x => x.GetCustomAttributes<RegisterInterceptorAttribute>().Any())
       .Select(Activator.CreateInstance)
       .Cast<IInterceptor>();

        optionsBuilder.AddInterceptors(interceptors);
    }

    private void AddVersionPropertyForOptimisticConcurency(ModelBuilder modelBuilder)
    {
        var properties = this.GetType().GetProperties();

        foreach (var property in properties)
        {
            var setType = property.PropertyType;
            var isDbSet = setType.IsGenericType && typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition());
            if (!isDbSet)
            {
                continue;
            }

            var entityType = setType.GetGenericArguments().First();
            var isOptimisticLocable = typeof(IOptimisticLockable).IsAssignableFrom(entityType);
            if (!isOptimisticLocable)
            {
                continue;
            }

            modelBuilder.Entity(entityType)
                .Property(nameof(IOptimisticLockable.Version))
                .IsRowVersion();
        }
    }
}
