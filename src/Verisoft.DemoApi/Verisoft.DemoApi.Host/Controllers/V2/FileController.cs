using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.BlobStorage;

namespace Verisoft.DemoApi.Host.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class FileController : ControllerBase
{
    private readonly IBlobStorageService fileService;

    public FileController(IBlobStorageService fileService)
    {
        this.fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is not provided or empty.");
        }

        var result = await fileService.UploadFileAsync(file);
        return Ok(result);
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFileAsync(string fileName)
    {
        try
        {
            var (stream, contentType) = await fileService.DownloadFileAsync(fileName);
            return File(stream, contentType, fileName, enableRangeProcessing: true);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found.");
        }
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeleteFileAsync(string fileName)
    {
        try
        {
            await fileService.DeleteFileAsync(fileName);
            return Ok("File deleted successfully.");
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListFilesAsync()
    {
        var result = await fileService.ListFilesAsync();
        return Ok(result);
    }
}
