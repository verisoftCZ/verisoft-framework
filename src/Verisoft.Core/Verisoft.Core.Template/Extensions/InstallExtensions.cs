using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Template.Services;

namespace Verisoft.Core.Template.Extensions;

public static class InstallExtensions
{
    public static IServiceCollection AddTemplateService(this IServiceCollection services)
    {
        services.AddScoped<ITemplateService, TemplateService>();
        return services;
    }
}