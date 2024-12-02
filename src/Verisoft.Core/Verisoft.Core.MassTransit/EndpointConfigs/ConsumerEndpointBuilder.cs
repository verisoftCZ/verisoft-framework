using MassTransit;

namespace Verisoft.Core.MassTransit.EndpointConfigs;

public class ConsumerEndpointBuilder(IRegistrationContext context, IReceiveEndpointConfigurator configurator)
{
    public ConsumerEndpointBuilder AddConsumer<TConsumer>()
        where TConsumer : class, IConsumer
    {
        configurator.ConfigureConsumer<TConsumer>(context);
        return this;
    }

    public ConsumerEndpointBuilder MaxRetries(int retryCount)
    {
        configurator.UseMessageRetry(r => r.Immediate(retryCount));
        return this;
    }
}
