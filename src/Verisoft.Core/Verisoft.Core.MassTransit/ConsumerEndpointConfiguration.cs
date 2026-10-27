using MassTransit;
using System;
using System.Collections.Generic;

namespace Verisoft.Core.MassTransit;

public class ConsumerEndpointConfiguration
{
    private readonly List<Action<IRegistrationContext, IReceiveEndpointConfigurator>> configActions = [];

    internal ConsumerEndpointConfiguration(Type consumerType, string endpointName)
    {
        ConsumerType = consumerType;
        EndpointName = endpointName;
    }

    internal ConsumerEndpointConfiguration(Type consumerType, Type consumerDefinitionType, string endpointName)
    {
        ConsumerType = consumerType;
        ConsumerDefinitionType = consumerDefinitionType;
        EndpointName = endpointName;
    }

    public Type ConsumerType { get; }

    public Type? ConsumerDefinitionType { get; }

    public string EndpointName { get; }

    public ConsumerEndpointConfiguration MaxRetries(int retryCount)
    {
        configActions.Add(
        (_, endpoint) => endpoint.UseMessageRetry(r => r.Immediate(retryCount)));
        return this;
    }

    public ConsumerEndpointConfiguration ConcurrencyLimit(int concurrency)
    {
        configActions.Add(
        (_, endpoint) => endpoint.ConcurrentMessageLimit = concurrency);
        return this;
    }

    internal void ApplyConfigurations(IRegistrationContext context, IReceiveEndpointConfigurator endpoint)
    {
        foreach (var action in configActions)
        {
            action(context, endpoint);
        }
    }
}