using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class PdfController(IDemoPdfService demoPdfService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<Stream>(StatusCodes.Status200OK)]
    public async Task<IActionResult?> GeneratePdfAsync(string pdfData)
    {
        var res = await demoPdfService.DownloadPdfAsync(pdfData);
        return res.ToActionResult();
    }
}