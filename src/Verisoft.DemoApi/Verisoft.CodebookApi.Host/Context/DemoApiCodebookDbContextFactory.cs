using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.CodebookApi.Host.Context;

// Lets 'dotnet ef' tooling create the context without bootstrapping the full API host (Key Vault, ...).
public class DemoApiCodebookDbContextFactory : IDesignTimeDbContextFactory<DemoApiCodebookDbContext>
{
    public DemoApiCodebookDbContext CreateDbContext(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddUserSecrets<DemoApiCodebookDbContextFactory>(optional: true)
            .AddEnvironmentVariables();

        // e.g. dotnet ef migrations add Name ... -- "Server=...;"
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:ConnectionString"] = args[0],
            });
        }

        var configuration = configurationBuilder.Build();

        if (string.IsNullOrWhiteSpace(configuration["Database:ConnectionString"]))
        {
            throw new InvalidOperationException(
                "Database:ConnectionString is not configured. Set it via 'dotnet user-secrets set \"Database:ConnectionString\" \"...\"' " +
                "(run in this project's folder), the Database__ConnectionString environment variable, or pass it after '--' to the dotnet ef command.");
        }

        var databaseConfig = new DatabaseConfig(configuration);

        var codebookRegistry = new CodebookRegistry("Records", "Codebook.");
        foreach (var codebookType in CodebookDiscovery.DiscoverFromAssembly(typeof(CodebookParentEntity).Assembly))
        {
            typeof(CodebookRegistry)
                .GetMethod(nameof(CodebookRegistry.RegisterCodebook))!
                .MakeGenericMethod(codebookType)
                .Invoke(codebookRegistry, null);
        }

        return new DemoApiCodebookDbContext(databaseConfig, codebookRegistry);
    }
}
