namespace Verisoft.Core.AspNet.Health;

public class HealthConfiguration
{
    public string? ConnectionString { get; set; }

    public string? DatabaseName { get; set; }

    public HealthDatabaseProvider Provider { get; set; } = HealthDatabaseProvider.SqlServer;
}
