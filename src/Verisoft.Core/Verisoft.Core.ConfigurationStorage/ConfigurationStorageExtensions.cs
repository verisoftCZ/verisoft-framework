using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Verisoft.Core.Data.EntityFramework;

namespace Verisoft.Core.ConfigurationStorage;

public sealed class ConfigurationStorageOptions
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Context { get; set; } = "default";

    [Required]
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
        where TContext : EntityFrameworkDbContextBase
    {
        services.AddOptions<ConfigurationStorageOptions>()
            .Configure(options => configure?.Invoke(options))
            .ValidateDataAnnotations()
            .Validate(options => options.Context is null
                || string.Equals(options.Context, options.Context.Trim(), StringComparison.Ordinal), "Context must not have leading or trailing whitespace.")
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
        services.TryAddScoped<IComponentConfigurationRepository>(provider => new ComponentConfigurationRepository(
            contextFactory is null ? provider.GetRequiredService<TContext>() : contextFactory(provider)));
        services.TryAddScoped<IConfigurationStorage, ConfigurationStorageService>();
        return services;
    }

    public static ModelBuilder AddConfigurationStorage(this ModelBuilder modelBuilder)
    {
        return modelBuilder.ApplyConfiguration(new ComponentConfigurationMapping());
    }
}