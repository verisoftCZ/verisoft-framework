using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Verisoft.Core.Data.EntityFramework.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureEnumAsString<TEntity, TEnum>(
       this EntityTypeBuilder<TEntity> builder,
       Expression<Func<TEntity, TEnum>> propertyExpression)
       where TEntity : class
       where TEnum : struct, Enum
    {
        builder.Property(propertyExpression)
               .HasConversion(
                   v => v.ToString(),
                   v => (TEnum)Enum.Parse(typeof(TEnum), v));

        return builder;
    }

    public static void HasManyWithMany<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression,
        Expression<Func<TRelatedEntity, IEnumerable<TEntity>>> inverseNavigationExpression,
        string foreignKey1,
        string foreignKey2,
        string tableName)
        where TEntity : class
        where TRelatedEntity : class
    {
        builder.HasMany(navigationExpression)
            .WithMany(inverseNavigationExpression)
            .UsingEntity<Dictionary<string, object>>(
                tableName,
                right => right.HasOne<TRelatedEntity>()
                    .WithMany()
                    .HasForeignKey(foreignKey1)
                    .OnDelete(DeleteBehavior.NoAction),
                left => left.HasOne<TEntity>()
                    .WithMany()
                    .HasForeignKey(foreignKey2)
                    .OnDelete(DeleteBehavior.NoAction),
                joinEntity =>
                {
                    joinEntity.ToTable(tableName);
                    joinEntity.HasKey(foreignKey1, foreignKey2);
                    joinEntity.HasIndex(foreignKey1);
                    joinEntity.HasIndex(foreignKey2);
                });
    }
}
