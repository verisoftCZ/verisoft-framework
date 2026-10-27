using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Codebooks.Common.Attributes;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.Extensions;

public static class EntityTypeBuilderExtensions
{
    private const string NullableAttribute = "System.Runtime.CompilerServices.NullableAttribute";
    private const int NullableReferenceType = 2;
    private const string NullableFlags = "NullableFlags";

    public static EntityTypeBuilder<TEntity> ConfigureOwnedType<TEntity, TOwnedType>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TOwnedType?>> ownedNavigationExpression)
        where TEntity : class
        where TOwnedType : class
    {
        var typeInfo = typeof(TOwnedType);
        var properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        builder.OwnsOne(ownedNavigationExpression, ownedBuilder =>
        {
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("stringValue", StringComparison.OrdinalIgnoreCase) ||
                    property.Name.EndsWith("CodebookRecord", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var parameter = Expression.Parameter(typeInfo, "x");
                var member = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(member, parameter);

                if (property.GetCustomAttribute<GenerateCodebookInfrastructureAttribute>() != null)
                {
                    ConfigureOwnedTypeCodebookRecord(property, ownedBuilder, lambda);
                }
                else
                {
                    ConfigureOwnedTypeProperty(property, ownedBuilder, lambda);
                }
            }
        });

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureCodebookRecord<TEntity, TCodebookRecordType>(
       this EntityTypeBuilder<TEntity> builder,
       Expression<Func<TEntity, CodebookRecord<TCodebookRecordType>?>> codebookRecordPropertyExpression)
       where TEntity : class
       where TCodebookRecordType : CodebookRecordType, new()
    {
        (string propertyName, string foreignKeyPropertyName, bool isNullable, string codebookRecordEntityPropertyName) = ConfigureCodebookRecordInternal(codebookRecordPropertyExpression);
        builder.Ignore(propertyName);

        builder.Property(foreignKeyPropertyName)
            .HasColumnName(propertyName)
            .HasMaxLength(CodebookRecordEntityConstants.StringValueMaxLength)
            .HasComment($"CodebookType: {typeof(TCodebookRecordType).Name}")
            .IsRequired(!isNullable);

        builder.HasOne(typeof(CodebookRecordEntity<TCodebookRecordType>), codebookRecordEntityPropertyName)
            .WithMany()
            .HasPrincipalKey(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue))
            .HasForeignKey(foreignKeyPropertyName)
            .OnDelete(DeleteBehavior.Restrict);

        return builder;
    }

    public static OwnedNavigationBuilder<TOwnerEntity, TEntity> ConfigureCodebookRecord<TOwnerEntity, TEntity, TCodebookRecordType>(
       this OwnedNavigationBuilder<TOwnerEntity, TEntity> builder,
       Expression<Func<TEntity, CodebookRecord<TCodebookRecordType>?>> codebookRecordPropertyExpression)
       where TOwnerEntity : class
       where TEntity : class
       where TCodebookRecordType : CodebookRecordType, new()
    {
        (string propertyName, string foreignKeyPropertyName, bool isNullable, string codebookRecordEntityPropertyName) = ConfigureCodebookRecordInternal(codebookRecordPropertyExpression);

        builder.Ignore(propertyName);

        builder.Property(foreignKeyPropertyName)
            .HasColumnName(propertyName)
            .HasMaxLength(CodebookRecordEntityConstants.StringValueMaxLength)
            .HasComment($"CodebookType: {typeof(TCodebookRecordType).Name}")
            .IsRequired(!isNullable);

        builder.HasOne(typeof(CodebookRecordEntity<TCodebookRecordType>), codebookRecordEntityPropertyName)
            .WithMany()
            .HasPrincipalKey(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue))
            .HasForeignKey(foreignKeyPropertyName)
            .OnDelete(DeleteBehavior.Restrict);

        return builder;
    }

    private static (string PropertyName, string ForeignKeyPropertyName, bool IsNullable, string CodebookRecordEntityPropertyName) ConfigureCodebookRecordInternal<TEntity, TCodebookRecordType>(Expression<Func<TEntity, CodebookRecord<TCodebookRecordType>?>> codebookRecordPropertyExpression)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var propertyName = ((MemberExpression)codebookRecordPropertyExpression.Body).Member.Name;
        var propertyInfo = typeof(TEntity).GetProperty(propertyName) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TEntity).Name}'.");

        _ = propertyInfo.GetCustomAttribute<GenerateCodebookInfrastructureAttribute>()
            ?? throw new InvalidOperationException(
                $"Configuration failed: Property '{typeof(TEntity).Name}.{propertyName}' is not marked with attribute [{nameof(GenerateCodebookInfrastructureAttribute)}] and cannot be configured using '{nameof(ConfigureCodebookRecord)}'. Add attribute or use custom configuration.");

        var codebookRecordEntityPropertyName = string.Concat(propertyName, CodebookRecordEntityConstants.CodebookRecordEntityPropertySuffix);
        _ = typeof(TEntity).GetProperty(codebookRecordEntityPropertyName) ?? throw new ArgumentException($"Property '{codebookRecordEntityPropertyName}' not found on type '{typeof(TEntity).Name}'. Check [{nameof(GenerateCodebookInfrastructureAttribute)}] which is responsible for creation of such property");

        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(propertyInfo);
        bool isNullable = nullabilityInfo.WriteState == NullabilityState.Nullable;
        var foreignKeyPropertyName = GetAndValidateCodebookRecordForeignKeyPropertyName(typeof(TEntity), ((MemberExpression)codebookRecordPropertyExpression.Body).Member.Name);

        return (propertyName, foreignKeyPropertyName, isNullable, codebookRecordEntityPropertyName);
    }

    private static string GetAndValidateCodebookRecordForeignKeyPropertyName(Type entityType, string codebookRecordPublicPropertyName)
    {
        if (string.IsNullOrEmpty(codebookRecordPublicPropertyName))
        {
            throw new ArgumentNullException(nameof(codebookRecordPublicPropertyName));
        }

        string expectedPropertyName = $"{codebookRecordPublicPropertyName}{CodebookRecordEntityConstants.CodebookRecordForeignKeyPropertySuffix}";

        var propertyInfo = entityType.GetProperty(expectedPropertyName, BindingFlags.Instance | BindingFlags.Public)
            ?? throw new InvalidOperationException(
                $"Convention for CodebookRecord failed: For public property '{codebookRecordPublicPropertyName}' of entity '{entityType.Name}' expected foreign key property name '{expectedPropertyName}' wasn't found. Check property name");

        if (propertyInfo.PropertyType != typeof(string))
        {
            throw new InvalidOperationException(
               $"Convention for CodebookRecord failed: Expected public property '{expectedPropertyName}' of entity '{entityType.Name}' is not of type 'string', but '{propertyInfo.PropertyType.Name}'.");
        }

        return expectedPropertyName;
    }

    private static void ConfigureOwnedTypeCodebookRecord<TEntity, TOwnedType>(PropertyInfo property, OwnedNavigationBuilder<TEntity, TOwnedType> ownedBuilder, LambdaExpression lambda)
        where TEntity : class
        where TOwnedType : class
    {
        var codebookType = property.PropertyType.GetGenericArguments()[0];

        var method = typeof(EntityTypeBuilderExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m =>
                m.Name == nameof(ConfigureCodebookRecord) &&
                m.IsGenericMethod &&
                m.GetGenericArguments().Length == 3);

        var genericMethod = method.MakeGenericMethod(typeof(TEntity), typeof(TOwnedType), codebookType);
        genericMethod.Invoke(null, [ownedBuilder, lambda]);
    }

    private static void ConfigureOwnedTypeProperty<TEntity, TOwnedType>(PropertyInfo property, OwnedNavigationBuilder<TEntity, TOwnedType> ownedBuilder, LambdaExpression lambda)
        where TEntity : class
        where TOwnedType : class
    {
        var propertyMethod = typeof(OwnedNavigationBuilder<,>)
            .MakeGenericType(typeof(TEntity), typeof(TOwnedType))
            .GetMethods()
            .First(m => m.Name == "Property" && m.IsGenericMethod);

        var genericPropertyMethod = propertyMethod.MakeGenericMethod(property.PropertyType);
        var propertyBuilder = genericPropertyMethod.Invoke(ownedBuilder, [lambda]);

        var columnMethod = typeof(RelationalPropertyBuilderExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m =>
                m.Name == nameof(RelationalPropertyBuilderExtensions.HasColumnName) &&
                m.IsGenericMethod &&
                m.GetParameters().Length == 2 &&
                m.GetParameters()[1].ParameterType == typeof(string));

        var genericColumnMethod = columnMethod?.MakeGenericMethod(property.PropertyType);
        genericColumnMethod?.Invoke(null, [propertyBuilder, property.Name]);
    }
}