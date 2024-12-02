using MassTransit;
using Verisoft.Core.MassTransit.EndpointConfigs;
using Verisoft.DemoApi.Application.Consumers;

namespace Verisoft.DemoApi.Application.Helpers;

public static class MassTransitEndpointConfigurator
{
    public static void ConfigureConsumerEndpoints(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        ConfigureCreateDocumentEndpoint(cfg, context);
    }

    private static void ConfigureCreateDocumentEndpoint(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ReceiveEndpoint("verisoft-demoApi-application-consumers-create-document", e =>
        {
            new ConsumerEndpointBuilder(context, e)
                .AddConsumer<CreateDocumentConsumer>()
                .MaxRetries(3);
        });
    }
}
