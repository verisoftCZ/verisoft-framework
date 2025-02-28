using System;
using System.Collections.Generic;
using MassTransit;

namespace Verisoft.Core.MassTransit;

public class VerisoftServiceBusOptions
{
    private readonly Dictionary<Type, ConsumerEndpointConfiguration> consumerEndpoints = new ();

    public ConsumerEndpointConfiguration ConfigureConsumer<TConsumer>(string endpointName = default)
        where TConsumer : class, IConsumer
    {
        endpointName ??= GenerateDefaultEndpointName(typeof(TConsumer));

        var config = new ConsumerEndpointConfiguration(typeof(TConsumer), endpointName);
        consumerEndpoints[typeof(TConsumer)] = config;
        return config;
    }

    internal IReadOnlyDictionary<Type, ConsumerEndpointConfiguration> GetConsumerConfigurations() => consumerEndpoints;

    private static string GenerateDefaultEndpointName(Type consumerType)
    {
        var fullName = consumerType.FullName ?? consumerType.Name;
        var sanitized = fullName.Replace(".", "-", StringComparison.OrdinalIgnoreCase);

        if (sanitized.EndsWith("Consumer", StringComparison.OrdinalIgnoreCase))
        {
            sanitized = sanitized[..^"Consumer".Length];
        }

        return sanitized.ToLowerInvariant();
    }
}