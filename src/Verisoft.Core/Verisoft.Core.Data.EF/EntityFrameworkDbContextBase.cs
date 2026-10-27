using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Atributes;
using Verisoft.Core.Data.EntityFramework.Extensions;
using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.Data.EntityFramework;

public abstract class EntityFrameworkDbContextBase : DbContext, IUnitOfWork, IMigrable
{
    private readonly IUserContext userContext;
    private readonly TimeProvider timeProvider;
    private readonly CascadeWalker cascadeWalker;

    protected EntityFrameworkDbContextBase(IUserContext userContext, IWalkerFactory walkerFactory, TimeProvider timeProvider)
    {
        this.userContext = userContext;
        this.timeProvider = timeProvider;
        this.cascadeWalker = walkerFactory.CreateCascadeWalker(this);
    }

    protected EntityFrameworkDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    public void Commit()
    {
        SaveChanges();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public void Migrate()
    {
        Database.Migrate();
    }

    public void Rollback()
    {
        this.RollbackChanges();
    }

    public override int SaveChanges()
    {
        ProcessSoftDeletesAsync().GetAwaiter().GetResult();
        ProcessAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await ProcessSoftDeletesAsync();
        ProcessAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected virtual void ProcessAuditFields()
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var userName = userContext.GetUserName();

        foreach (var entry in ChangeTracker.Entries())
        {
            ApplyAuditInfo(entry, now, userName);
        }
    }

    protected async Task ProcessSoftDeletesAsync()
    {
        var deletedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is IBaseEntity)
            .ToList();

        foreach (var entry in deletedEntities)
        {
            if (entry.Entity is IBaseEntity baseEntity)
            {
                entry.State = EntityState.Modified;
                baseEntity.IsDeleted = true;

                var entityType = Model.FindEntityType(entry.Entity.GetType());

                if (entityType != null)
                {
                    await cascadeWalker.WalkAndSoftDeleteAsync(entry.Entity, entityType);
                }
            }
        }
    }

    protected virtual string AssignUser(string? existingUser, string newUser)
    {
        return newUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddSoftDeleteQueryFilterToBaseEntities(modelBuilder);
        AddVersionPropertyForOptimisticConcurrency(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Conventions.Add(serviceProvider =>
            new SoftDeleteUniqueIndexConvention(
                serviceProvider.GetService<ISqlGenerationHelper>(),
                serviceProvider.GetService<IRelationalTypeMappingSource>()));
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

#if NET10_0_OR_GREATER
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter("SoftDelete", lambda);
#else
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
#endif
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

    protected virtual void ApplyAuditInfo(EntityEntry entry, DateTime now, string userName)
    {
        if (entry.Entity is IBaseEntity baseEntity)
        {
            if (entry.State is EntityState.Added)
            {
                baseEntity.CreatedBy = userName;
                baseEntity.CreatedAt = now;
            }
            else if (entry.State is EntityState.Modified or EntityState.Deleted)
            {
                baseEntity.UpdatedBy = AssignUser(baseEntity.UpdatedBy, userName);
                baseEntity.UpdatedAt = now;
            }
        }
    }
}
