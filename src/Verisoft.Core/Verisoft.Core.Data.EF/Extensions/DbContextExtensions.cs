using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Verisoft.Core.Data.EntityFramework.Extensions;

public static class DbContextExtensions
{
    private const string TableNameAnnotationName = "Relational:TableName";
    private const string SchemaAnnotationName = "Relational:Schema";
    private const string DefaultSchema = "dbo";

    public static void RollbackChanges(this DbContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Detached && entry.State != EntityState.Unchanged)
            {
                entry.State = EntityState.Unchanged;
            }
        }
    }

    public static string GetTableName<TEntity>(this EntityFrameworkDbContextBase dbContext)
        where TEntity : class
    {
        return dbContext.GetAnnotationStringValueByAnnotationNameFromDbSet<TEntity>(TableNameAnnotationName)
               ?? dbContext.GetDbSetPropertyByEntityType<TEntity>()?.Name;
    }

    public static string GetTableSchema<TEntity>(this EntityFrameworkDbContextBase dbContext)
        where TEntity : class
    {
        return dbContext.GetAnnotationStringValueByAnnotationNameFromDbSet<TEntity>(SchemaAnnotationName)
               ?? dbContext.DefaultSchema
               ?? DefaultSchema;
    }

    private static string GetAnnotationStringValueByAnnotationNameFromDbSet<TEntity>(this EntityFrameworkDbContextBase dbContext, string annotationName)
       where TEntity : class
    {
        var dbSetProperty = GetDbSetPropertyByEntityType<TEntity>(dbContext);

        if (dbSetProperty != null)
        {
            var dbSet = (DbSet<TEntity>)dbSetProperty.GetValue(dbContext);
            var annotationsFromDbSet = dbSet.EntityType.GetAnnotations();
            return annotationsFromDbSet.SingleOrDefault(x => x.Name.Contains(annotationName))?.Value?.ToString();
        }

        return null;
    }

    private static PropertyInfo GetDbSetPropertyByEntityType<TEntity>(this EntityFrameworkDbContextBase dbContext)
        where TEntity : class
    {
        return dbContext.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>));
    }
}
