using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Verisoft.Core.BlobStorage.InstallExtensions;

public static class InstallExtensions
{
    public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        AddBlobServiceClient(services, configuration);

        var containerName = configuration["AzureBlobStorage:AzureBlobContainerName"];
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentException("Container name must be specified in AzureBlobStorage:AzureBlobContainerName");
        }

        services.AddScoped<IBlobStorageService, BlobStorageService>(x => new BlobStorageService(x.GetRequiredService<BlobServiceClient>(), containerName));
    }

    private static void AddBlobServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(connectionString));
            return;
        }

        var tenantId = configuration["AzureAd:TenantId"];
        var clientId = configuration["AzureAd:ClientId"];
        var clientSecret = configuration["AzureAd:ClientSecret"];
        var accountName = configuration["AzureBlobStorage:AccountName"];
        if (string.IsNullOrWhiteSpace(tenantId) ||
            string.IsNullOrWhiteSpace(clientId) ||
            string.IsNullOrWhiteSpace(clientSecret) ||
            string.IsNullOrWhiteSpace(accountName))
        {
            throw new InvalidOperationException("Either AzureBlobStorage:AzureBlobConnectionString or AzureAd credentials must be provided to configure BlobServiceClient. Neither one was.");
        }

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        services.AddSingleton(_ => new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net"), credential));
    }
}
