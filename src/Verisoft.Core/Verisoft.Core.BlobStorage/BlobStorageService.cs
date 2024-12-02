using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Verisoft.Core.BlobStorage;

public class BlobStorageService(BlobServiceClient blobServiceClient, string containerName) : IBlobStorageService
{
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(file.FileName);
        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.ToString();
    }

    public async Task<(Stream? Stream, string? ContentType)> DownloadFileAsync(string fileName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            return (null, null);
        }

        var response = await blobClient.DownloadAsync();
        return (response.Value.Content, response.Value.ContentType);
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<IEnumerable<string>> ListFilesAsync()
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = containerClient.GetBlobsAsync();
        var fileList = new List<string>();

        await foreach (var blobItem in blobs)
        {
            fileList.Add(blobItem.Name);
        }

        return fileList;
    }
}