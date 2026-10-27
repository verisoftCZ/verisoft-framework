using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Verisoft.DemoApi.Contracts.Commands;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
public class MassTransitController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost("publish/Consumer")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PublishExportJobConsumer(CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();

        var command = new CreateDocumentCommand(documentId, "demo.txt", 1);
        await publishEndpoint.Publish(
            command,
            context =>
            {
                context.Headers.Set("RequestId", Guid.NewGuid().ToString());
                context.Headers.Set("Priority", "High");
            },
            cancellationToken);

        return Accepted(new { DocuemntId = documentId, message = "Job successfully queued" });
    }

    [HttpPost("publish/Saga/Start")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PublishExportJobSagaStart(CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();

        // Using the saga-specific command with more information
        var command = new InitiateDocumentSagaCommand(
            documentId,
            "saga-document.txt",
            1,
            DateTime.UtcNow);

        await publishEndpoint.Publish(
            command,
            context =>
            {
                context.Headers.Set("RequestId", Guid.NewGuid().ToString());
                context.Headers.Set("Priority", "High");
                context.Headers.Set("ProcessType", "Saga");
            },
            cancellationToken);

        return Accepted(new { DocumentId = documentId, message = "Saga process initiated successfully" });
    }

    [HttpPost("publish/Saga/Complete")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PublishExportJobSagaComplete(CancellationToken cancellationToken, Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            return BadRequest(new { message = "Invalid or missing documentId." });
        }

        var command = new CompleteDocumentSagaCommand(documentId);

        await publishEndpoint.Publish(
            command,
            context =>
            {
                context.Headers.Set("RequestId", Guid.NewGuid().ToString());
                context.Headers.Set("Priority", "High");
                context.Headers.Set("ProcessType", "Saga");
            },
            cancellationToken);

        return Accepted(new { DocumentId = documentId, message = "Saga process completed successfully" });
    }
}