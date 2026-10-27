using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.Core.Export;

public interface IExportStrategy<TEntity>
{
    public Task<ExportFileResult> ExportAsync(IEnumerable<TEntity> entities, ExportBuilder<TEntity>? settings);

    public bool CanHandle(ExportType exportType);
}
