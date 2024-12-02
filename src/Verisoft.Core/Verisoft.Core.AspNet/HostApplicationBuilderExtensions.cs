using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Verisoft.Core.AspNet
{
    public static class HostApplicationBuilderExtensions
    {
        public static IHostApplicationBuilder AddAzureKeyVault(this IHostApplicationBuilder app, IConfiguration configuration)
        {
            app.Configuration.AddAzureKeyVault(
                new Uri(configuration["KeyVault:Uri"]),
                new ClientSecretCredential(
                    configuration["AzureAd:TenantId"],
                    configuration["AzureAd:ClientId"],
                    configuration["AzureAd:ClientSecret"])
                );
            return app;
        }
    }
}
