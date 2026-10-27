using MassTransit;
using System;
using System.Collections.Generic;

namespace Verisoft.Core.MassTransit
{
    public class SagaEndpointConfiguration
    {
        private readonly List<Action<IRegistrationContext, IReceiveEndpointConfigurator>> configActions = new();

        internal SagaEndpointConfiguration(Type sagaStateMachineType, Type sagaStateType, string endpointName)
        {
            SagaStateMachineType = sagaStateMachineType;
            SagaStateType = sagaStateType;
            EndpointName = endpointName;
        }

        public Type SagaStateMachineType { get; }

        public Type SagaStateType { get; }

        public string EndpointName { get; }

        public SagaEndpointConfiguration MaxRetries(int retryCount)
        {
            configActions.Add((_, e) => e.UseMessageRetry(r => r.Immediate(retryCount)));
            return this;
        }

        public SagaEndpointConfiguration ConcurrencyLimit(int concurrency)
        {
            configActions.Add((_, e) => e.ConcurrentMessageLimit = concurrency);
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
}
