using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework.Walkers;

public class CascadeWalker(EntityFrameworkDbContextBase context, ILogger<CascadeWalker> logger)
{
    private readonly EntityFrameworkDbContextBase context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<CascadeWalker> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly HashSet<object> processedEntities = [];

    public async Task WalkAndSoftDeleteAsync(object entity, IEntityType entityType)
    {
        if (entity == null || processedEntities.Contains(entity))
        {
            return;
        }

        processedEntities.Add(entity);

        await ValidateRestrictConstraintsAsync(entity, entityType);
        await ProcessSetNullReferencesAsync(entity, entityType);

        if (entity is IBaseEntity baseEntity && !baseEntity.IsDeleted)
        {
            baseEntity.IsDeleted = true;

            logger.LogDebug("Soft deleted {entityTypeName} with ID: {entityId}", entityType.ClrType.Name, GetEntityId(entity, entityType));
        }

        await ProcessCascadeForeignKeysAsync(entity, entityType);
    }

    private static List<IForeignKey> GetCascadeForeignKeys(IEntityType entityType)
    {
        var cascadeForeignKeys = new List<IForeignKey>();

        foreach (var foreignKey in entityType.GetReferencingForeignKeys())
        {
            if (foreignKey.DeleteBehavior == DeleteBehavior.Cascade ||
                foreignKey.DeleteBehavior == DeleteBehavior.ClientCascade)
            {
                cascadeForeignKeys.Add(foreignKey);
            }
        }

        return cascadeForeignKeys;
    }

    private static object GetEntityId(object entity, IEntityType entityType)
    {
        var keyProperty = entityType.FindPrimaryKey()?.Properties.FirstOrDefault();
        return keyProperty?.PropertyInfo?.GetValue(entity) ?? "Unknown";
    }

    private static object? GetCurrentPropertyValue(EntityEntry entry, object entity, string propertyName, PropertyInfo? propertyInfo)
    {
        if (entry.State != EntityState.Detached)
        {
            return entry.Property(propertyName).CurrentValue;
        }

        return propertyInfo?.GetValue(entity);
    }

    private static bool MatchesCurrentForeignKey(EntityEntry entry, IForeignKey foreignKey, object?[] principalKeyValues)
    {
        for (int i = 0; i < foreignKey.Properties.Count; i++)
        {
            var currentValue = entry.Property(foreignKey.Properties[i].Name).CurrentValue;
            if (!Equals(currentValue, principalKeyValues[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsActiveEntity(EntityEntry entry)
    {
        if (!typeof(IBaseEntity).IsAssignableFrom(entry.Metadata.ClrType))
        {
            return true;
        }

        return entry.Property(nameof(IBaseEntity.IsDeleted)).CurrentValue is not true;
    }

    private object?[] GetKeyValues(object entity, IKey key)
    {
        var entry = context.Entry(entity);
        return [.. key.Properties.Select(property => GetCurrentPropertyValue(entry, entity, property.Name, property.PropertyInfo))];
    }

    private List<object> GetTrackedDependentEntities(IForeignKey foreignKey, object?[] principalKeyValues)
    {
        var dependentType = foreignKey.DeclaringEntityType.ClrType;

        return [.. context.ChangeTracker.Entries()
            .Where(entry => entry.State != EntityState.Detached)
            .Where(entry => entry.State != EntityState.Deleted)
            .Where(entry => dependentType.IsAssignableFrom(entry.Entity.GetType()))
            .Where(entry => MatchesCurrentForeignKey(entry, foreignKey, principalKeyValues))
            .Where(IsActiveEntity)
            .Select(entry => entry.Entity)];
    }

    private async Task ProcessCascadeForeignKeysAsync(object entity, IEntityType entityType)
    {
        var cascadeForeignKeys = GetCascadeForeignKeys(entityType);

        foreach (var foreignKey in cascadeForeignKeys)
        {
            await ProcessCascadeForeignKeyAsync(entity, foreignKey);
        }
    }

    private async Task ValidateRestrictConstraintsAsync(object entity, IEntityType entityType)
    {
        var restrictForeignKeys = context.Model.GetEntityTypes()
            .SelectMany(et => et.GetForeignKeys())
            .Where(fk => fk.PrincipalEntityType == entityType &&
                        fk.DeleteBehavior == DeleteBehavior.Restrict)
            .ToList();

        var restrictionViolations = new List<string>();

        foreach (var foreignKey in restrictForeignKeys)
        {
            var violationMessage = await CheckRestrictConstraintAsync(entity, foreignKey);
            if (!string.IsNullOrEmpty(violationMessage))
            {
                restrictionViolations.Add(violationMessage);
            }
        }

        if (restrictionViolations.Count != 0)
        {
            var entityId = GetEntityId(entity, entityType);
            var entityName = entityType.ClrType.Name;

            var violationDetails = string.Join("; ", restrictionViolations);

            logger.LogWarning("Cannot delete {entityTypeName} (ID: {entityId}) due to restrict constraints: {violationDetails}", entityName, entityId, violationDetails);
            throw new EntityDeleteBlockedException(entityType.ClrType, entityId, restrictionViolations);
        }
    }

    private async Task<string?> CheckRestrictConstraintAsync(object principalEntity, IForeignKey foreignKey)
    {
        try
        {
            var dependentEntityType = foreignKey.DeclaringEntityType;
            var principalKey = foreignKey.PrincipalKey;

            var principalKeyValues = GetKeyValues(principalEntity, principalKey);

            if (principalKeyValues.Any(v => v == null))
            {
                return null;
            }

            var dbSetProperty = context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                               p.PropertyType.GetGenericArguments()[0] == dependentEntityType.ClrType);

            var dependentEntities = dbSetProperty?.GetValue(context) is IQueryable queryable
                ? await FindDependentEntitiesAsync(queryable, foreignKey, principalKeyValues)
                : GetTrackedDependentEntities(foreignKey, principalKeyValues);

            if (dependentEntities.Count != 0)
            {
                var dependentTypeName = dependentEntityType.ClrType.Name;
                return $"{dependentEntities.Count} active {dependentTypeName} record(s)";
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking restrict constraint for {foreignKeyDeclaringEntityTypeName}", foreignKey.DeclaringEntityType.ClrType.Name);
            return $"Error validating {foreignKey.DeclaringEntityType.ClrType.Name} constraint";
        }

        return null;
    }

    private async Task<List<object>> FindDependentEntitiesAsync(
        IQueryable queryable, IForeignKey foreignKey, object?[] principalKeyValues)
    {
        try
        {
            var entities = await FindDependentEntitiesInDatabaseAsync(queryable, foreignKey, principalKeyValues);

            foreach (var trackedEntity in GetTrackedDependentEntities(foreignKey, principalKeyValues))
            {
                if (!entities.Contains(trackedEntity))
                {
                    entities.Add(trackedEntity);
                }
            }

            return entities;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in FindDependentEntities");
        }

        return [];
    }

    private async Task<List<object>> FindDependentEntitiesInDatabaseAsync(
        IQueryable queryable, IForeignKey foreignKey, object?[] principalKeyValues)
    {
        var dependentType = foreignKey.DeclaringEntityType.ClrType;
        var parameter = Expression.Parameter(dependentType, "x");

        Expression? condition = null;
        for (int i = 0; i < foreignKey.Properties.Count; i++)
        {
            var fkProperty = foreignKey.Properties[i];
            var propertyAccess = Expression.Property(parameter, fkProperty.Name);
            var constantValue = principalKeyValues[i];

            Expression constant;
            if (propertyAccess.Type != constantValue?.GetType())
            {
                if (propertyAccess.Type.IsGenericType && propertyAccess.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var nullableType = propertyAccess.Type;
                    constant = Expression.Convert(Expression.Constant(constantValue), nullableType);
                }
                else if (constantValue?.GetType().IsGenericType == true && constantValue.GetType().GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    constant = Expression.Convert(Expression.Constant(constantValue), propertyAccess.Type);
                }
                else
                {
                    constant = Expression.Constant(constantValue, propertyAccess.Type);
                }
            }
            else
            {
                constant = Expression.Constant(constantValue);
            }

            var equal = Expression.Equal(propertyAccess, constant);
            condition = condition == null ? equal : Expression.AndAlso(condition, equal);
        }

        if (typeof(IBaseEntity).IsAssignableFrom(dependentType))
        {
            var isDeletedProperty = Expression.Property(parameter, nameof(IBaseEntity.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var notDeleted = Expression.Equal(isDeletedProperty, falseConstant);
            condition = condition == null ? notDeleted : Expression.AndAlso(condition, notDeleted);
        }

        var whereMethod = typeof(Queryable).GetMethods()
            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 2)
            ?.MakeGenericMethod(dependentType);

        if (whereMethod == null || condition == null)
        {
            return [];
        }

        var lambda = Expression.Lambda(condition, parameter);
        var filteredQuery = whereMethod.Invoke(null, [queryable, lambda]);

        var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m => m.Name == "ToListAsync" && m.GetParameters().Length == 2)?
            .MakeGenericMethod(dependentType);

        if (toListAsyncMethod == null)
        {
            return [];
        }

        var task = (Task?)toListAsyncMethod.Invoke(null, [filteredQuery, CancellationToken.None]);

        if (task == null)
        {
            return [];
        }

        await task;

        var resultProperty = task.GetType().GetProperty("Result");
        if (resultProperty?.GetValue(task) is not IEnumerable result)
        {
            return [];
        }

        var entities = new List<object>();
        foreach (var item in result)
        {
            var entry = context.Entry(item);
            if (MatchesCurrentForeignKey(entry, foreignKey, principalKeyValues) && IsActiveEntity(entry))
            {
                entities.Add(item);
            }
        }

        return entities;
    }

    private async Task ProcessSetNullReferencesAsync(object entity, IEntityType entityType)
    {
        var incomingForeignKeys = context.Model.GetEntityTypes()
            .SelectMany(et => et.GetForeignKeys())
            .Where(fk => fk.PrincipalEntityType == entityType &&
                        (fk.DeleteBehavior == DeleteBehavior.SetNull || fk.DeleteBehavior == DeleteBehavior.ClientSetNull))
            .ToList();

        foreach (var foreignKey in incomingForeignKeys)
        {
            await ProcessSetNullForForeignKeyAsync(entity, foreignKey);
        }
    }

    private async Task ProcessSetNullForForeignKeyAsync(object principalEntity, IForeignKey foreignKey)
    {
        try
        {
            var dependentEntityType = foreignKey.DeclaringEntityType;
            var principalKey = foreignKey.PrincipalKey;

            var principalKeyValues = GetKeyValues(principalEntity, principalKey);

            if (principalKeyValues.Any(v => v == null))
            {
                return;
            }

            var dbSetProperty = context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                               p.PropertyType.GetGenericArguments()[0] == dependentEntityType.ClrType);

            var dependentEntities = dbSetProperty?.GetValue(context) is IQueryable queryable
                ? await FindDependentEntitiesAsync(queryable, foreignKey, principalKeyValues)
                : GetTrackedDependentEntities(foreignKey, principalKeyValues);

            foreach (var dependentEntity in dependentEntities)
            {
                foreach (var fkProperty in foreignKey.Properties)
                {
                    var propertyInfo = fkProperty.PropertyInfo;
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        var propertyType = propertyInfo.PropertyType;
                        var isNullable = !propertyType.IsValueType ||
                            (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));

                        if (isNullable)
                        {
                            var oldValue = propertyInfo.GetValue(dependentEntity);
                            propertyInfo.SetValue(dependentEntity, null);

                            logger.LogInformation("Set {dependentEntityTypeName}.{foreignKeyPropertyName} from {oldValue} to NULL", dependentEntityType.ClrType.Name, fkProperty.Name, oldValue);

                            var entry = context.Entry(dependentEntity);
                            if (entry.State == EntityState.Detached)
                            {
                                context.Attach(dependentEntity);
                            }

                            entry.Property(fkProperty.Name).IsModified = true;
                        }
                        else
                        {
                            logger.LogWarning("Cannot set {dependentEntityTypeName}.{foreignKeyPropertyName} to NULL - property is not nullable", dependentEntityType.ClrType.Name, fkProperty.Name);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing SetNull for foreign key {foreignKeyDeclaringEntityTypeName}", foreignKey.DeclaringEntityType.ClrType.Name);
        }
    }

    private async Task ProcessCascadeForeignKeyAsync(object principalEntity, IForeignKey foreignKey)
    {
        try
        {
            var dependentEntityType = foreignKey.DeclaringEntityType;
            var principalKey = foreignKey.PrincipalKey;

            var principalKeyValues = GetKeyValues(principalEntity, principalKey);

            if (principalKeyValues.Any(v => v == null))
            {
                return;
            }

            var dbSetProperty = context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                               p.PropertyType.GetGenericArguments()[0] == dependentEntityType.ClrType);

            var dependentEntities = dbSetProperty?.GetValue(context) is IQueryable queryable
                ? await FindDependentEntitiesAsync(queryable, foreignKey, principalKeyValues)
                : GetTrackedDependentEntities(foreignKey, principalKeyValues);

            foreach (var dependentEntity in dependentEntities)
            {
                if (dependentEntity is IBaseEntity baseEntity && !baseEntity.IsDeleted)
                {
                    await WalkAndSoftDeleteAsync(dependentEntity, dependentEntityType);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing cascade FK {foreignKeyDeclaringEntityTypeName}", foreignKey.DeclaringEntityType.ClrType.Name);
        }
    }
}
