using MassTransit;
using Microsoft.Extensions.Logging;
using Verisoft.DemoApi.Contracts.Commands;

namespace Verisoft.DemoApi.Application.Consumers;

public class CreateDocumentConsumer(ILogger<CreateDocumentCommand> logger) : IConsumer<CreateDocumentCommand>
{
    public Task Consume(ConsumeContext<CreateDocumentCommand> context)
    {
        var request = context.Message;
        logger.LogInformation("$Document Created: {RequestDocumentId} Name: {RequestDocumentName}",
            request.DocumentId,
            request.DocumentName);

        return Task.CompletedTask;
    }
}