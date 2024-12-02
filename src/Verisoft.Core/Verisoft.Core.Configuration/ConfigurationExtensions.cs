using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Verisoft.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void AddConfiguration<TInterface, TClass>(this IServiceCollection services, string? name = null)
            where TClass : class, TInterface
            where TInterface : class
        {
            services.AddOptions<TClass>()
                .BindConfiguration(name ?? typeof(TClass).Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton<TInterface>(c =>
            {
                var options = c.GetRequiredService<IOptions<TClass>>();
                return options.Value;
            });
        }
    }
}
