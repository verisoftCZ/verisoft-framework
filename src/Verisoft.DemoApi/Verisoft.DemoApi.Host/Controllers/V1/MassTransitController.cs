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
    [HttpPost("publish")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PublishExportJob(CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();

        var command = new CreateDocumentCommand(documentId, "demo.txt", 1);
        await publishEndpoint.Publish(command,
            context =>
            {
                context.Headers.Set("RequestId", Guid.NewGuid().ToString());
                context.Headers.Set("Priority", "High");
            },
            cancellationToken);

        return Accepted(new { DocuemntId = documentId, message = "Job successfully queued" });
    }
}