using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Codebooks.Common.Constants;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Extenders;

namespace Verisoft.Core.Codebooks.Common.Entities.Configurations;

public static class CodebookRecordConfiguration
{
    public static void ApplyCodebookRecordConfigurations(this ModelBuilder modelBuilder, ICodebookRegistry codebookRegistry, bool excludeFromMigrations = false)
    {
        var codebookTypes = typeof(CodebookRecordType).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(CodebookRecordType).IsAssignableFrom(t))
            .ToList();

        foreach (var codebookType in codebookTypes)
        {
            var genericType = typeof(CodebookRecord<>).MakeGenericType(codebookType);
            modelBuilder.Ignore(genericType);
        }

        foreach (var codebookRecordType in codebookRegistry.GetRegisteredCodebooks())
        {
            var codebookType = codebookRecordType;
            if (!typeof(CodebookRecordType).IsAssignableFrom(codebookType))
            {
                throw new InvalidOperationException($"Type {codebookType.Name} is not a CodebookRecordType");
            }

            var configureCodebookRecordEntityMethodInfo = typeof(CodebookRecordConfiguration)
                .GetMethod(nameof(ConfigureCodebookRecordEntity), BindingFlags.Static | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Method {nameof(ConfigureCodebookRecordEntity)} was not found.");

            configureCodebookRecordEntityMethodInfo.MakeGenericMethod(codebookType).Invoke(null, [modelBuilder, codebookRegistry.CodebookRecordTablePrefix, codebookRecordType.Name, excludeFromMigrations]);

            var configureTranslationEntityMethodInfo = typeof(CodebookRecordConfiguration)
                .GetMethod(nameof(ConfigureTranslationEntity), BindingFlags.Static | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Method {nameof(ConfigureTranslationEntity)} was not found.");

            configureTranslationEntityMethodInfo.MakeGenericMethod(codebookType).Invoke(null, [modelBuilder, codebookRegistry.CodebookRecordTablePrefix, codebookRecordType.Name, excludeFromMigrations]);
        }
    }

    private static void ConfigureCodebookRecordEntity<TCodebookRecordType>(ModelBuilder modelBuilder, string codebookRecordTablePrefix, string codebookRecordTableName, bool excludeFromMigrations = false)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var codebookType = typeof(TCodebookRecordType);
        var entityType = typeof(CodebookRecordEntity<TCodebookRecordType>);
        modelBuilder.Entity(entityType, builder =>
        {
            var tableName = $"{codebookRecordTablePrefix}{codebookRecordTableName}";

            if (excludeFromMigrations)
            {
                builder.ToTable(tableName, SchemaConstants.CodebookSchema, x => x.ExcludeFromMigrations());
            }
            else
            {
                builder.ToTable(tableName, SchemaConstants.CodebookSchema);
            }

            builder.HasKey(nameof(CodebookRecordEntity<TCodebookRecordType>.Id));
            builder.HasAlternateKey(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue));
            builder.Property(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue)).HasMaxLength(CodebookRecordEntityConstants.StringValueMaxLength).IsRequired();
            builder.Property(nameof(CodebookRecordEntity<TCodebookRecordType>.Description)).HasMaxLength(CodebookRecordEntityConstants.DescriptionValueMaxLength);
            builder.Property<DateOnly>(nameof(CodebookRecordEntity<TCodebookRecordType>.ValidFrom));
            builder.Property<DateOnly?>(nameof(CodebookRecordEntity<TCodebookRecordType>.ValidTo));
            ConfigureAdditionalProperties<TCodebookRecordType>(builder);
        });
    }

    private static void ConfigureAdditionalProperties<TCodebookRecordType>(EntityTypeBuilder builder)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var codebookType = typeof(TCodebookRecordType);
        var codebookTypeProperties = codebookType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.DeclaringType != typeof(CodebookRecordType)).ToList();

        if (codebookTypeProperties.Count == 0)
        {
            builder.Ignore(nameof(CodebookRecordEntity<TCodebookRecordType>.AdditionalProperties));
        }
        else
        {
            builder.OwnsOne(codebookType, nameof(CodebookRecordEntity<TCodebookRecordType>.AdditionalProperties), ownedBuilder =>
            {
                foreach (var property in codebookTypeProperties)
                {
                    var isNullable = property.IsNullableType();
                    var propertyType = property.PropertyType;
                    var underlyingType = (isNullable ? Nullable.GetUnderlyingType(propertyType) : propertyType) ?? propertyType;

                    var ownedPropertyBuilder = ownedBuilder.Property(underlyingType, property.Name)
                    .HasColumnName(property.Name)
                    .IsRequired(!isNullable);

                    if (underlyingType == typeof(decimal))
                    {
                        ownedPropertyBuilder
                        .HasColumnType("decimal(18,2)")
                        .HasPrecision(18, 2);
                    }
                }
            });

            if (codebookTypeProperties.Any(p => !p.IsNullableType()))
            {
                builder.Navigation(nameof(CodebookRecordEntity<TCodebookRecordType>.AdditionalProperties)).IsRequired();
            }
        }
    }

    private static void ConfigureTranslationEntity<TCodebookRecordType>(ModelBuilder modelBuilder, string codebookRecordTablePrefix, string codebookRecordTableName, bool excludeFromMigrations = false)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var codebookType = typeof(TCodebookRecordType);
        var translationType = typeof(CodebookRecordTranslationEntity<TCodebookRecordType>);

        modelBuilder.Entity(translationType, builder =>
        {
            var tableName = $"{codebookRecordTablePrefix}{codebookRecordTableName}Translation";

            if (excludeFromMigrations)
            {
                builder.ToTable(tableName, SchemaConstants.CodebookSchema, x => x.ExcludeFromMigrations());
            }
            else
            {
                builder.ToTable(tableName, SchemaConstants.CodebookSchema);
            }

            builder.HasKey(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.Id));
            builder.Property(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.Value)).HasMaxLength(CodebookRecordTranslationEntityConstants.ValueMaxLength).IsRequired();
            builder.Property(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.LanguageCode)).HasMaxLength(CodebookRecordTranslationEntityConstants.LanguageCodeMaxLength).IsRequired();
            builder.Property<int>(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.CodebookRecordId))
                .IsRequired();

            builder.HasOne(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.CodebookRecord))
            .WithMany(CodebookRecordEntityConstants.Translations)
            .HasForeignKey(nameof(CodebookRecordTranslationEntity<TCodebookRecordType>.CodebookRecordId))
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}