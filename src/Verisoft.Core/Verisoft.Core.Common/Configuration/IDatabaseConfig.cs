namespace Verisoft.Core.Common.Configuration
{
    public interface IDatabaseConfig
    {
        string ConnectionString { get; }

        string DefaultSchema { get; }
    }
}
