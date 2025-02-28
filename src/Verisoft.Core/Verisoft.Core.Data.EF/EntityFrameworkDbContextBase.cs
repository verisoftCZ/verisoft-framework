using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Atributes;
using Verisoft.Core.Data.EntityFramework.Extensions;

namespace Verisoft.Core.Data.EntityFramework;

public abstract class EntityFrameworkDbContextBase : DbContext, IUnitOfWork, IMigrable
{
    protected EntityFrameworkDbContextBase()
    {
    }

    protected EntityFrameworkDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    public void Commit()
    {
        SaveChanges();
    }

    public void Migrate()
    {
        Database.Migrate();
    }

    public void Rollback()
    {
        this.RollbackChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddSoftDeleteQueryFilterToBaseEntities(modelBuilder);
        AddVersionPropertyForOptimisticConcurrency(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        RegisterInterceptorsByAttribute(optionsBuilder);
    }

    private static void RegisterInterceptorsByAttribute(DbContextOptionsBuilder optionsBuilder)
    {
        var interceptors = AppDomain.CurrentDomain.GetAssemblies()
           .Where(assembly => assembly.FullName != null && assembly.FullName.StartsWith("Verisoft"))
           .SelectMany(assembly => assembly.GetTypes())
           .Where(x => x.IsClass && x.IsPublic)
           .Where(x => x.GetCustomAttributes<RegisterInterceptorAttribute>().Any())
           .Select(Activator.CreateInstance)
           .Cast<IInterceptor>();

        optionsBuilder.AddInterceptors(interceptors);
    }

    private static void AddSoftDeleteQueryFilterToBaseEntities(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(IBaseEntity.IsDeleted));
            var notExpression = Expression.Not(property);
            var lambda = Expression.Lambda(notExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    private void AddVersionPropertyForOptimisticConcurrency(ModelBuilder modelBuilder)
    {
        var properties = GetType().GetProperties();

        foreach (var property in properties)
        {
            var setType = property.PropertyType;
            var isDbSet = setType.IsGenericType && typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition());
            if (!isDbSet)
            {
                continue;
            }

            var entityType = setType.GetGenericArguments().First();
            var isOptimisticLockable = typeof(IOptimisticLockable).IsAssignableFrom(entityType);
            if (!isOptimisticLockable)
            {
                continue;
            }

            modelBuilder.Entity(entityType)
                .Property(nameof(IOptimisticLockable.Version))
                .IsRowVersion();
        }
    }
}
