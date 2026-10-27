using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace Verisoft.Core.KeyVault
{
    public static class AzureKeyVaultExtensionsBase
    {
        private const string KeyVaultEnabledKey = "KeyVault:Enabled";
        private const string KeyVaultAllowInteractiveCredentialKey = "KeyVault:AllowInteractiveCredential";
        private const string KeyVaultAllowInsecureTlsKey = "KeyVault:AllowInsecureTls";
        private const string KeyVaultDisableChallengeResourceVerificationKey = "KeyVault:DisableChallengeResourceVerification";
        private const string KeyVaultUriKey = "KeyVault:Uri";
        private const string AzureadClientSecret = "AzureAd:ClientSecret";
        private const string AzureAdTenantId = "AzureAd:TenantId";
        private const string AzureAdClientId = "AzureAd:ClientId";
        private const string AzureAdAuthorityHost = "AzureAd:AuthorityHost";

        /// <summary>
        /// Adds Azure Key Vault configuration to the application builder.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure.</param>
        /// <param name="configuration">The application's configuration, used to retrieve Key Vault settings.</param>
        /// <param name="appSettingsOverride"> If appsettings should override Key Vault secrets.</param>
        /// <param name="allowManagedIdentity"> If managed identity should be used.</param>
        /// A boolean value indicating whether to override app settings with Key Vault secrets.
        /// If <c>true</c>, a custom <see cref="KeyVaultOverridePolicy"/> is applied.
        /// </param>
        /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if required configuration values (e.g., "KeyVault:Uri", "AzureAd:TenantId", "AzureAd:ClientId", or "AzureAd:ClientSecret") are missing.
        /// </exception>
        public static WebApplicationBuilder AddAzureKeyVault(this WebApplicationBuilder builder, IConfiguration configuration, bool appSettingsOverride = true, bool allowManagedIdentity = false)
        {
            if (configuration.GetValue<bool?>(KeyVaultEnabledKey) == false)
            {
                Console.WriteLine("Key Vault configuration is disabled.");
                return builder;
            }

            if (configuration[KeyVaultUriKey] == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Key Vault URI is required.");
            }

            var uri = new Uri(configuration[KeyVaultUriKey]!);
            var runningInContainer = string.Equals(
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
                "true",
                StringComparison.OrdinalIgnoreCase);
            var allowInsecureTls = configuration.GetValue<bool?>(KeyVaultAllowInsecureTlsKey) == true;
            HttpPipelineTransport? insecureTransport = null;
            if (allowInsecureTls)
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                };

                insecureTransport = new HttpClientTransport(handler);
            }

            TokenCredential credential;
            var authorityHost = configuration[AzureAdAuthorityHost];

            if (configuration[AzureadClientSecret] != null)
            {
                var credentialOptions = new ClientSecretCredentialOptions();
                if (!string.IsNullOrWhiteSpace(authorityHost))
                {
                    credentialOptions.AuthorityHost = new Uri(authorityHost);
                    credentialOptions.DisableInstanceDiscovery = true;
                }

                if (insecureTransport is not null)
                {
                    credentialOptions.Transport = insecureTransport;
                }

                credentialOptions.AdditionallyAllowedTenants.Add("*");

                credential = new ClientSecretCredential(
                    configuration[AzureAdTenantId] ?? throw new ArgumentNullException(nameof(configuration), "AzureAd:TenantId is required when AzureAd:ClientSecret is configured."),
                    configuration[AzureAdClientId] ?? throw new ArgumentNullException(nameof(configuration), "AzureAd:ClientId is required when AzureAd:ClientSecret is configured."),
                    configuration[AzureadClientSecret],
                    credentialOptions);
            }
            else
            {
                // Developer mode: managed identity → VS → CLI → browser
                var creds = new List<TokenCredential>();

                if (allowManagedIdentity)
                {
                    creds.Add(new ManagedIdentityCredential());
                }

                // Visual Studio / Visual Studio Code
                creds.Add(new VisualStudioCredential());
                creds.Add(new VisualStudioCodeCredential());

                // Azure CLI (logged‑in user via `az login`)
                creds.Add(new AzureCliCredential());

                if (!runningInContainer && configuration.GetValue<bool?>(KeyVaultAllowInteractiveCredentialKey) != false)
                {
                    // Interactive browser fallback for local desktop development only.
                    creds.Add(new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
                    {
                        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                    }));
                }

                credential = new ChainedTokenCredential(creds.ToArray());
            }

            var options = new AzureKeyVaultConfigurationOptions();

            if (appSettingsOverride)
            {
                options.Manager = new KeyVaultOverridePolicy(configuration);
            }

            var secretClientOptions = new SecretClientOptions();
            var isAzureManagedVaultHost = uri.Host.EndsWith(".vault.azure.net", StringComparison.OrdinalIgnoreCase);
            if (configuration.GetValue<bool?>(KeyVaultDisableChallengeResourceVerificationKey) == true || !isAzureManagedVaultHost)
            {
                secretClientOptions.DisableChallengeResourceVerification = true;
            }

            if (insecureTransport is not null)
            {
                secretClientOptions.Transport = insecureTransport;
            }

            var secretClient = new SecretClient(uri, credential, secretClientOptions);
            builder.Configuration.AddAzureKeyVault(secretClient, options);

            return builder;
        }
    }
}