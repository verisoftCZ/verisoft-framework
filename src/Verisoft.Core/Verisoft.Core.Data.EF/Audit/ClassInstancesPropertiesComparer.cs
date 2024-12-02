using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework.Audit;

public class ClassInstancesPropertiesComparer
{
    private const char ElementIdsSeparator = ';';
    private const string DateTimeDatabaseFormat = "yyyy-MM-dd HH:mm:ss";
    private static readonly List<string> DefaultIgnoredPropertiesToAudit =
    [
        "UpdatedBy",
        "UpdatedAt",
    ];

    public static List<EntityAuditDetailEntity> GetChangesInCommonProperties<TEntity, TUpdatedEntity>(TEntity entity, TUpdatedEntity updatedEntity, List<string> ignoredProperties = null)
        where TEntity : class
        where TUpdatedEntity : class
    {
        var changes = new List<EntityAuditDetailEntity>();

        if (entity is null || updatedEntity is null)
        {
            return changes;
        }

        ignoredProperties ??= [];

        changes.AddRange(GetChangesInCollection(entity, updatedEntity, ignoredProperties));

        ignoredProperties.AddRange(DefaultIgnoredPropertiesToAudit);
        ignoredProperties.AddRange(GetClassPropertyNames(entity));

        var commonProperties = GetCommonProperties<TEntity, TUpdatedEntity>().ExceptBy(ignoredProperties, x => x.Name);

        foreach (var property in commonProperties)
        {
            var entityPropertyValue = GetPropertyValue(entity, property.Name);
            var updatedEntitylPropertyValue = GetPropertyValue(updatedEntity, property.Name);

            if (property.PropertyType == typeof(decimal) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(decimal))
            {
                var entityDecimalValue = entityPropertyValue is not null ? Convert.ToDecimal(entityPropertyValue) : (decimal?)null;
                var updatedEntityDecimalValue = updatedEntitylPropertyValue is not null ? Convert.ToDecimal(updatedEntitylPropertyValue) : (decimal?)null;

                if (entityDecimalValue != updatedEntityDecimalValue)
                {
                    changes.Add(new EntityAuditDetailEntity
                    {
                        ChangedAttribute = property.Name,
                        OldValue = entityDecimalValue?.ToString(CultureInfo.CurrentCulture),
                        NewValue = updatedEntityDecimalValue?.ToString(CultureInfo.CurrentCulture),
                    });
                }

                continue;
            }

            var entityValueString = ConvertToString(entityPropertyValue);
            var updatedEntitylValueString = ConvertToString(updatedEntitylPropertyValue);

            if (entityValueString != updatedEntitylValueString)
            {
                changes.Add(new EntityAuditDetailEntity
                {
                    ChangedAttribute = property.Name,
                    OldValue = entityValueString,
                    NewValue = updatedEntitylValueString,
                });
            }
        }

        return changes;
    }

    private static List<EntityAuditDetailEntity> GetChangesInCollection<TEntity, TUpdatedEntity>(TEntity entity, TUpdatedEntity updatedEntity, List<string> ignoredProperties)
    {
        var commonCollections = GetCommonProperties<TEntity, TUpdatedEntity>(true);
        var changesInCollections = new List<EntityAuditDetailEntity>();
        foreach (var collection in commonCollections.Where(c => !ignoredProperties.Contains(c.Name)))
        {
            var collectionName = collection.Name;
            PropertyInfo entityCollectionProperty = typeof(TEntity).GetProperty(collectionName);
            PropertyInfo updatedEntityCollectionProperty = typeof(TUpdatedEntity).GetProperty(collectionName);

            if (entityCollectionProperty is not null && entityCollectionProperty.PropertyType.IsGenericType && updatedEntityCollectionProperty is not null && updatedEntityCollectionProperty.PropertyType.IsGenericType)
            {
                Type elementType = entityCollectionProperty.PropertyType.GetGenericArguments()[0];

                if (typeof(IEntity).IsAssignableFrom(elementType))
                {
                    IEnumerable<IEntity> entityCollection = (IEnumerable<IEntity>)entityCollectionProperty.GetValue(entity);
                    IEnumerable<IEntity> updatedEntityCollection = (IEnumerable<IEntity>)updatedEntityCollectionProperty.GetValue(updatedEntity);

                    var idsInEntityCollection = string.Join(ElementIdsSeparator, entityCollection.OrderBy(x => x.Id).Select(x => x.Id.ToString()));
                    var idsInUpdatedEntityCollection = string.Join(ElementIdsSeparator, updatedEntityCollection.OrderBy(x => x.Id).Select(x => x.Id.ToString()));

                    if (idsInEntityCollection != idsInUpdatedEntityCollection)
                    {
                        changesInCollections.Add(new EntityAuditDetailEntity()
                        {
                            ChangedAttribute = collectionName,
                            OldValue = idsInEntityCollection,
                            NewValue = idsInUpdatedEntityCollection,
                        });
                    }
                }
            }
        }

        return changesInCollections;
    }

    private static IEnumerable<PropertyInfo> GetCommonProperties<TEntity, TUpdatedEntity>(bool collections = false)
    {
        var entityProperties = typeof(TEntity).GetProperties();
        var updatedEntityProperties = typeof(TUpdatedEntity).GetProperties();

        return entityProperties
        .Join(
            updatedEntityProperties,
            ep => new { ep.Name, PropertyType = NormalizeType(ep.PropertyType) },
            mp => new { mp.Name, PropertyType = NormalizeType(mp.PropertyType) },
            (ep, mp) => ep)
        .Where(property => IsCollectionType(property.PropertyType) == collections);
    }

    private static Type NormalizeType(Type propertyType)
    {
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Nullable.GetUnderlyingType(propertyType);
        }

        return propertyType;
    }

    private static object GetPropertyValue<T>(T entity, string propertyName)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property name {propertyName} was not found in {typeof(T).Name}.");
        }

        return propertyInfo.GetValue(entity) ?? null;
    }

    private static string ConvertToString(object value)
    {
        if (value is DateTime dateTimeValue)
        {
            return dateTimeValue.ToString(DateTimeDatabaseFormat);
        }
        else if (value is DateTime?)
        {
            var nullableDateTimeValue = (DateTime?)value;
            if (nullableDateTimeValue.HasValue)
            {
                return nullableDateTimeValue.Value.ToString(DateTimeDatabaseFormat);
            }
        }

        return value?.ToString() ?? "null";
    }

    private static bool IsCollectionType(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(ICollection<>) || type.GetGenericTypeDefinition() == typeof(HashSet<>));
    }

    private static List<string> GetClassPropertyNames<T>(T classInstance)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return properties
            .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string))
            .Select(p => p.Name)
            .ToList();
    }
}
