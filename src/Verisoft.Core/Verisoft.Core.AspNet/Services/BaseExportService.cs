using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.AspNet.Services;

public abstract class BaseExportService<TListItem, TFilter, TSpecification, TSort, TEntity, TKey>(
    ITypeMapper typeMapper,
    IRepository<TEntity, TKey> repository,
    IExportService<TListItem> exportService)
    : IBaseExportService<TListItem, TFilter, TKey>
    where TEntity : class, IBaseEntity<TKey>, IEntity
    where TSpecification : ISpecification<TEntity>, new()
    where TFilter : new()
    where TSort : BaseSort<TEntity>, new()
{
    private readonly IRepository<TEntity, TKey> repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IExportService<TListItem> exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));

    protected ITypeMapper TypeMapper { get; } = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));

    public virtual async Task<ExportFileResult> ExportDataAsync(ExportDataRequest<TFilter> request)
    {
        var requestFilter = request.Filter ?? new TFilter();
        var specification = TypeMapper.Map<TSpecification>(requestFilter) ?? new TSpecification();
        var filteringExpression = specification.SatisfiedBy();
        var entities = await repository.GetFilteredSortForExportAsync<TSort>(filteringExpression, request.Sorts);
        var dataToExport = MapToExportableData(entities);
        return await exportService.ExportDataAsync(dataToExport, request.ColumnsToExport);
    }

    protected virtual IEnumerable<TListItem> MapToExportableData(IEnumerable<TEntity> entities)
        => TypeMapper.Map<TListItem>(entities);
}