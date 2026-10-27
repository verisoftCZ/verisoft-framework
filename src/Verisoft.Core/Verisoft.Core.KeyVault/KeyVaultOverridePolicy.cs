
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Verisoft.Core.KeyVault
{
    /// <summary>
    /// A custom Key Vault secret manager that overrides app settings with Key Vault secrets.
    /// </summary>
    /// <param name="configuration">
    /// The application's configuration, used to retrieve existing app settings for comparison and override.
    /// </param>
    internal class KeyVaultOverridePolicy(IConfiguration configuration) : KeyVaultSecretManager
    {
        private readonly IConfiguration configuration = configuration;
        public override Dictionary<string, string> GetData(IEnumerable<KeyVaultSecret> secrets)
        {
            var baseData = base.GetData(secrets);

            var updatedData = new Dictionary<string, string>();

            foreach (var secret in baseData)
            {
                var confValue = configuration[secret.Key];
                updatedData[secret.Key] = !string.IsNullOrEmpty(confValue) ? confValue : secret.Value;
            }
            return updatedData;
        }
    }

}
