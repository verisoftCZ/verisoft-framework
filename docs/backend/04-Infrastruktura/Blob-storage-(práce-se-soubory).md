Pro práci se soubory v cloud prostředí není vhodné používat file systém z důvodů distribuovaných systémů.
Pro kontejnerové aplikace je většinou file systém problém jelikož s koncem života kontejneru umírá i file systém a není perzistentní.

Pro ukládání souborů využíváme Azure Blob storage.
Konkrétně jde o `Verisoft.Core.BlobStorage`

Pro registraci do aplikace:
    
``` csharp
private static void RegisterBlobStorage(IServiceCollection serviceCollection, IConfiguration configuration)
{
     serviceCollection.AddBlobStorage(configuration);
}
```

Je nutné nastavit konfigurace:

``` js
"ConnectionStrings" : {
  "AzureBlobStorage": "connection string to storage"   //TOTO by ideálně mělo být v key vault
},
"AzureBlobStorage": {
  "AzureBlobContainerName": "azure-webjobs-hosts"
}
```

Interface:
    
``` csharp
public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<(Stream, string)> DownloadFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<IEnumerable<string>> ListFilesAsync();
    }
```

Demo controller:
``` csharp
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

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is not provided or empty.");
        }

        var result = await fileService.UploadFileAsync(file);
        return Ok(result);
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
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

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteFile(string fileName)
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

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        var result = await fileService.ListFilesAsync();
        return Ok(result);
    }
}
```


