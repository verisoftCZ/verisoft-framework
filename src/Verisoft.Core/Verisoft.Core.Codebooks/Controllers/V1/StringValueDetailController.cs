using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Codebooks.Common.Models.Bulk;
using Verisoft.Core.Codebooks.Services;

namespace Verisoft.Core.Codebooks.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[Authorize]
[ProducesResponseType<BadRequestResult>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class StringValueDetailController(IBulkStringValueService bulkStringValueService) : Controller
{
    private readonly IBulkStringValueService bulkStringValueService = bulkStringValueService ?? throw new ArgumentNullException(nameof(bulkStringValueService));

    [HttpPost]
    [Route("ImportStringValueDetailFromExcelFile")]
    [ProducesResponseType<BulkImportResult>(StatusCodes.Status200OK)]
    public async Task<BulkImportResult> ImportStringValueDetailFromExcelFileAsync(IFormFile file)
    {
        return await bulkStringValueService.ImportStringValueDetailFromExcelFileAsync(file);
    }
}