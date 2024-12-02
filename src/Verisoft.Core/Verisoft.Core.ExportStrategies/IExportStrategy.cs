using Verisoft.Core.ExportStrategies.Enums;

namespace Verisoft.Core.ExportStrategies;

public interface IExportStrategy<in TEntity>
{
    public Task<ExportResult.ExportResult> ExportAsync(IEnumerable<TEntity> entities);

    public bool CanHandle(ExportType exportType);
}
