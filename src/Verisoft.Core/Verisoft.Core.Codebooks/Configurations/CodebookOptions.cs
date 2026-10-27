using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Codebooks.Authorization;

namespace Verisoft.Core.Codebooks.Configurations;

public sealed class CodebookOptions
{
    internal Action<IServiceCollection>? ConfigurePermissionChecker { get; private set; }

    // TODO: implement permission checker

    /// <summary>
    /// Configure Codebooks authorization to use permissions.
    /// </summary>
    //public void UsePermissions()
    //    => ConfigurePermissionChecker = services =>
    //        services.AddScoped<ICodebookAuthorizationChecker, CodebookPermissionChecker>();

    /// <summary>
    /// Configure Codebooks authorization to use roles.
    /// </summary>
    public void UseRoles()
        => ConfigurePermissionChecker = services =>
            services.AddScoped<ICodebookAuthorizationChecker, CodebookRoleChecker>();

    /// <summary>
    /// Configures Codebooks authorization to use a custom permission checker.
    /// </summary>
    /// <typeparam name="T">
    /// The implementation of <see cref="ICodebookAuthorizationChecker"/> that will be registered in the dependency injection container.
    /// </typeparam>
    public void UsePermissionChecker<T>()
        where T : class, ICodebookAuthorizationChecker
        => ConfigurePermissionChecker = services =>
            services.AddScoped<ICodebookAuthorizationChecker, T>();
}