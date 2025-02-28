using MassTransit;
using Verisoft.DemoApi.Contracts.Commands;

namespace Verisoft.DemoApi.Host.Consumers;

public class CreateDocumentConsumer(ILogger<CreateDocumentCommand> logger) : IConsumer<CreateDocumentCommand>
{
    public Task Consume(ConsumeContext<CreateDocumentCommand> context)
    {
        var request = context.Message;
        logger.LogInformation(
            "$Document Created: {RequestDocumentId} Name: {RequestDocumentName}",
            request.DocumentId,
            request.DocumentName);

        return Task.CompletedTask;
    }
}