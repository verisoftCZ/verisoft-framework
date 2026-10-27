using MassTransit.Configuration;

namespace Verisoft.Core.MassTransit;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddVerisoftServiceBus<T>(this IServiceCollection serviceCollection, IConfiguration configuration, Action<VerisoftServiceBusOptions> configure)
    {
        var options = new VerisoftServiceBusOptions();
        configure(options);

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
            var sagaRepositoryConfigurator = options.GetSagaRepositoryConfigurator();
            if (sagaRepositoryConfigurator != null)
            {
                sagaRepositoryConfigurator(c);
            }
            else
            {
                c.SetSagaRepositoryProvider(new InMemorySagaRepositoryRegistrationProvider());
            }

            foreach (var consumerConfig in options.GetConsumerConfigurations().Values)
            {
                c.AddConsumer(consumerConfig.ConsumerType, consumerConfig.ConsumerDefinitionType);
            }

            foreach (var sagaConfig in options.GetSagaConfigurations().Values)
            {
                c.AddSagaStateMachines(sagaConfig.SagaStateMachineType);
            }

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

                cfg.UsePublishFilter(typeof(AutoAuthorizationHeaderPublishFilter<>), context);
                cfg.UseConsumeFilter(typeof(UserContextPopulationConsumeFilter<>), context);

                foreach (var consumerConfig in options.GetConsumerConfigurations().Values)
                {
                    cfg.ReceiveEndpoint(consumerConfig.EndpointName, e =>
                    {
                        e.ConfigureConsumer(context, consumerConfig.ConsumerType);
                        consumerConfig.ApplyConfigurations(context, e);
                    });
                }

                foreach (var sagaConfig in options.GetSagaConfigurations().Values)
                {
                    cfg.ReceiveEndpoint(sagaConfig.EndpointName, e =>
                    {
                        e.ConfigureSaga(context, sagaConfig.SagaStateType);
                        sagaConfig.ApplyConfigurations(context, e);
                    });
                }

                cfg.ConfigureEndpoints(context);
            });
        });

        return serviceCollection;
    }
}