using Microsoft.EntityFrameworkCore;
using Verisoft.CodebookApi.Core.Configurations;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;

namespace Verisoft.CodebookApi.Database.Configurations;

public class CodebookRecordSchemaManager(IMainCodebookRecordConfiguration codebookRecordConfiguration) : ICodebookRecordSchemaManager
{
    public void ConfigureTables(ModelBuilder modelBuilder)
    {
        foreach (var codebookRecordConfig in codebookRecordConfiguration.CodebookRecordConfiguration)
        {
            var implementsICodebookRecordEntity = typeof(ICodebookRecordEntity).IsAssignableFrom(codebookRecordConfig.CodebookRecordType);

            if (!implementsICodebookRecordEntity)
            {
                throw new InvalidOperationException($"{codebookRecordConfig.CodebookRecordType.Name} does not implement {nameof(ICodebookRecordEntity)}");
            }

            modelBuilder.Entity(codebookRecordConfig.CodebookRecordType, x =>
            {
                x.ToTable($"{codebookRecordConfiguration.CodebookRecordTablePrefix}{codebookRecordConfig.CodebookRecordTableName}");
                x.HasKey(nameof(ICodebookRecordEntity.Id));
                x.Property(nameof(ICodebookRecordEntity.StringValue)).IsRequired();
                x.Property(nameof(ICodebookRecordEntity.Description)).HasMaxLength(255);
                x.Property(nameof(ICodebookRecordEntity.IsDefault));
                x.Property(nameof(ICodebookRecordEntity.IsGlobal));
                x.Ignore(nameof(ICodebookRecordEntity.Translations));
                x.Ignore(nameof(ICodebookRecordEntity.TenantValues));

                foreach (var property in codebookRecordConfig.CodebookRecordType.GetProperties())
                {
                    if (property.PropertyType == typeof(decimal))
                    {
                        x.Property(property.Name).HasColumnType("decimal(18,2)");
                    }
                }
            });
        }
    }
}