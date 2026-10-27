using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Verisoft.Core.Authentication;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.Core.Data.EntityFramework.Walkers;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Data.EF.Context;

// Lets 'dotnet ef' tooling create the context without bootstrapping the full API host (Key Vault, MassTransit, ...).
public class DemoApiDbContextFactory : IDesignTimeDbContextFactory<DemoApiDbContext>
{
    public DemoApiDbContext CreateDbContext(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddUserSecrets<DemoApiDbContextFactory>(optional: true)
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

        var walkerFactory = new WalkerFactory(NullLogger<CascadeWalker>.Instance);

        return new DemoApiDbContext(databaseConfig, codebookRegistry, new DesignTimeUserContext(), walkerFactory, TimeProvider.System);
    }

    private sealed class DesignTimeUserContext : IUserContext
    {
        public IPrincipal User { get; private set; } = new GenericPrincipal(new GenericIdentity("design-time"), []);

        public string GetUserName() => "design-time";

        public string? GetToken() => null;

        public void SetUser(IPrincipal user) => User = user;

        public void SetToken(string token)
        {
        }

        public void ClearInjectedUserAndToken()
        {
        }
    }
}
