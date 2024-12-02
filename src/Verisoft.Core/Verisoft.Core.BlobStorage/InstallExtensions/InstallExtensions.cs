using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Verisoft.Core.BlobStorage.InstallExtensions;

public static class InstallExtensions
{
    public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(x => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));
        var containerName = configuration["AzureBlobStorage:AzureBlobContainerName"];
        if (containerName == null)
        {
            throw new ArgumentException("Container name must be specified in AzureBlobStorage:AzureBlobContainerName");
        }

        services.AddScoped<IBlobStorageService, BlobStorageService>(x => new BlobStorageService(x.GetRequiredService<BlobServiceClient>(), containerName));
    }
}
