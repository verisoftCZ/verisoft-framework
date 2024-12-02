namespace Verisoft.Core.MassTransit;

using System;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class DependencyInjectionExtensions
{
    public static void AddServiceBus(this IServiceCollection serviceCollection, IConfiguration configuration, Action<IBusRegistrationConfigurator> registration = null, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> usingRabbitMqInjection = null)
    {
        var config = new MassTransitConfig(configuration);
        if (string.IsNullOrEmpty(config.Server?.Url))
        {
            throw new InvalidOperationException("The MassTransit url is not configured.");
        }

        serviceCollection.AddSingleton<IMassTransitConfig>(_ => config);
        serviceCollection.AddSingleton(_ => new MassTransitApplicationInsightsLoggerProvider(config));

        if (config.ApplicationInsights?.Enabled != false)
        {
            serviceCollection.AddSingleton<ILoggerProvider, MassTransitApplicationInsightsLoggerProvider>();
        }

        serviceCollection.AddMassTransit(c =>
        {
            c.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(true));

            c.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.Server.Url, hostConfig =>
                {
                    if (!string.IsNullOrEmpty(config.Server.UserName))
                    {
                        hostConfig.Username(config.Server.UserName);
                    }

                    if (!string.IsNullOrEmpty(config.Server.Password))
                    {
                        hostConfig.Password(config.Server.Password);
                    }
                });

                usingRabbitMqInjection?.Invoke(context, cfg);

                cfg.ConfigureEndpoints(context);
            });

            registration?.Invoke(c);
        });
    }
}