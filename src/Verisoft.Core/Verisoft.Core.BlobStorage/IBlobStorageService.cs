using Microsoft.AspNetCore.Http;

namespace Verisoft.Core.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file);

    Task<(Stream? Stream, string? ContentType)> DownloadFileAsync(string fileName);

    Task<bool> DeleteFileAsync(string fileName);

    Task<IEnumerable<string>> ListFilesAsync();
}