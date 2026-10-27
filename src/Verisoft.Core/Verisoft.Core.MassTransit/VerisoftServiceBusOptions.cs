using MassTransit;
using System;
using System.Collections.Generic;

namespace Verisoft.Core.MassTransit;

public class VerisoftServiceBusOptions
{
    private readonly Dictionary<Type, ConsumerEndpointConfiguration> consumerEndpoints = new();
    private readonly Dictionary<Type, SagaEndpointConfiguration> sagaEndpoints = new();

    private Action<IBusRegistrationConfigurator>? sagaRepositoryConfigurator;

    public ConsumerEndpointConfiguration ConfigureConsumer<TConsumer>(string? endpointName = default)
        where TConsumer : class, IConsumer
    {
        endpointName ??= GenerateDefaultEndpointName(typeof(TConsumer), "Consumer");

        var config = new ConsumerEndpointConfiguration(typeof(TConsumer), endpointName);
        consumerEndpoints[typeof(TConsumer)] = config;
        return config;
    }

    public ConsumerEndpointConfiguration ConfigureConsumer<TConsumer, TConsumerDefinition>(string? endpointName = default)
        where TConsumer : class, IConsumer
        where TConsumerDefinition : class, IConsumerDefinition
    {
        endpointName ??= GenerateDefaultEndpointName(typeof(TConsumer), "Consumer");

        var config = new ConsumerEndpointConfiguration(typeof(TConsumer), typeof(TConsumerDefinition), endpointName);
        consumerEndpoints[typeof(TConsumer)] = config;
        return config;
    }

    public SagaEndpointConfiguration ConfigureSaga<TSaga, TSagaState>(string? endpointName = default)
        where TSaga : MassTransitStateMachine<TSagaState>
        where TSagaState : class, SagaStateMachineInstance
    {
        endpointName ??= GenerateDefaultEndpointName(typeof(TSaga), "Saga");

        var config = new SagaEndpointConfiguration(typeof(TSaga), typeof(TSagaState), endpointName);
        sagaEndpoints[typeof(TSaga)] = config;
        return config;
    }

    public VerisoftServiceBusOptions SetSagaRepository(Action<IBusRegistrationConfigurator> configure)
    {
        sagaRepositoryConfigurator = configure ?? throw new ArgumentNullException(nameof(configure));
        return this;
    }

    internal Action<IBusRegistrationConfigurator>? GetSagaRepositoryConfigurator() => sagaRepositoryConfigurator;

    internal IReadOnlyDictionary<Type, ConsumerEndpointConfiguration> GetConsumerConfigurations() => consumerEndpoints;

    internal IReadOnlyDictionary<Type, SagaEndpointConfiguration> GetSagaConfigurations() => sagaEndpoints;

    private static string GenerateDefaultEndpointName(Type consumerType, string suffix)
    {
        var fullName = consumerType.FullName ?? consumerType.Name;
        var sanitized = fullName.Replace(".", "-", StringComparison.OrdinalIgnoreCase);

        if (sanitized.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            sanitized = sanitized[..^suffix.Length];
        }

        return sanitized.ToLowerInvariant();
    }
}