using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verisoft.Core.Codebooks.Common.Constants;

namespace Verisoft.Core.Codebooks.Common.Entities.Configurations;

public abstract class CodebookBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    protected string Schema { get; } = SchemaConstants.CodebookSchema;

    protected virtual bool IsView { get; } = false;

    protected virtual string? TableName { get; } = null;

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        if (!IsView)
        {
            builder.ToTable(TableName ?? GetTableNameFromEntityType(), Schema);
        }
        else
        {
            builder.ToView(TableName ?? GetTableNameFromEntityType(), Schema);
        }

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    private static string GetTableNameFromEntityType()
    {
        var typeName = typeof(TEntity).Name;
        return typeName.EndsWith("Entity") ? typeName[..^6] : throw new InvalidOperationException("Entity name must end with 'Entity'");
    }
}
