using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Verisoft.Core.Contracts.FailureDetail))]
public class PdfController(IDemoPdfService demoPdfService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stream))]
    public async Task<IActionResult> GeneratePdfAsync(string pdfData)
    {
        var res = await demoPdfService.DownloadPdfAsync(pdfData);
        return res.ToActionResult();
    }
}