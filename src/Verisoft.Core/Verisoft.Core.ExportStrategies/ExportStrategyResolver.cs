using Verisoft.Core.ExportStrategies.Enums;

namespace Verisoft.Core.ExportStrategies;

public class ExportStrategyResolver<TEntity>(IEnumerable<IExportStrategy<TEntity>> strategies)
{
    public IExportStrategy<TEntity> Resolve(ExportType exportType)
    {
        var strategy = strategies.FirstOrDefault(s => s.CanHandle(exportType));
        if (strategy == null)
        {
            throw new NotSupportedException($"Export type {exportType} is not supported for {typeof(TEntity).Name}.");
        }

        return strategy;
    }
}
