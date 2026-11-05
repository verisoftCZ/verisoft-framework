using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Verisoft.Core.ConfigurationStorage;

public sealed class ConfigurationStorageOptions
{
    public string Context { get; set; } = "default";

    public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;

    public const string AdminPolicy = "ConfigurationStorage.Admin";
}

public static class ConfigurationStorageExtensions
{
    public static IServiceCollection AddConfigurationStorage<TContext>(
        this IServiceCollection services,
        Action<ConfigurationStorageOptions>? configure = null,
        Action<AuthorizationPolicyBuilder>? configureAdminPolicy = null,
        Func<IServiceProvider, TContext>? contextFactory = null)
        where TContext : DbContext
    {
        services.AddOptions<ConfigurationStorageOptions>()
            .Configure(options => configure?.Invoke(options))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Context)
                && options.Context.Length <= 100
                && string.Equals(options.Context, options.Context.Trim(), StringComparison.Ordinal), "Context must contain 1 to 100 characters without leading or trailing whitespace.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.UserIdClaimType), "UserIdClaimType is required.")
            .ValidateOnStart();
        services.AddAuthorization(options => options.AddPolicy(ConfigurationStorageOptions.AdminPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            if (configureAdminPolicy is null)
            {
                policy.RequireRole("Admin");
            }
            else
            {
                configureAdminPolicy(policy);
            }
        }));
        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IConfigurationStorage>(provider => new ComponentConfigurationStorage<TContext>(
            contextFactory is null ? provider.GetRequiredService<TContext>() : contextFactory(provider),
            provider.GetRequiredService<Verisoft.Core.Authentication.IUserContext>(),
            provider.GetRequiredService<IAuthorizationService>(),
            provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ConfigurationStorageOptions>>(),
            provider.GetRequiredService<TimeProvider>()));
        return services;
    }

    public static ModelBuilder AddConfigurationStorage(this ModelBuilder modelBuilder)
    {
        return modelBuilder.ApplyConfiguration(new ComponentConfigurationMapping());
    }
}