using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework;

internal sealed class SoftDeleteUniqueIndexConvention(
    ISqlGenerationHelper? sql,
    IRelationalTypeMappingSource? typeMappings)
    : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        if (sql is null || typeMappings is null)
        {
            return;
        }

        var falseLiteral = typeMappings.GetMapping(typeof(bool)).GenerateSqlLiteral(false);

        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (!typeof(IBaseEntity).IsAssignableFrom(clrType)
                || entityType.FindProperty(nameof(IBaseEntity.IsDeleted)) is not { } isDeleted
                || isDeleted.GetColumnName() is not { } columnName)
            {
                continue;
            }

            var column = sql.DelimitIdentifier(columnName);
            var softDeleteFilter = $"{column} = {falseLiteral}";

            foreach (var index in entityType.GetIndexes().Where(index => index.IsUnique))
            {
                var existingFilter = index.GetFilter();

                var filter = string.IsNullOrWhiteSpace(existingFilter)
                    ? softDeleteFilter
                    : existingFilter.Contains(column, StringComparison.Ordinal)
                        ? null
                        : $"({existingFilter}) AND {softDeleteFilter}";

                if (filter is not null && index is IMutableIndex mutableIndex)
                {
                    mutableIndex.SetFilter(filter);
                }
            }
        }
    }
}
