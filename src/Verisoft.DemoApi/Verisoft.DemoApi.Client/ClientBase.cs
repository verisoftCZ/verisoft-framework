using System.Net.Http.Headers;

namespace Verisoft.DemoApi.Client;

public class ClientBase(IClientSettings settings)
{
    private readonly IClientSettings settings = settings ?? throw new ArgumentNullException(nameof(settings));

    protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken token)
    {
        await Task.Yield();

        if (settings.ServiceUrl == null)
        {
            throw new InvalidOperationException("Service url address is not set.");
        }

        var client = new HttpClient()
        {
            BaseAddress = new Uri(settings.ServiceUrl),
        };

        if (!string.IsNullOrEmpty(settings.AuthorizationToken))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", settings.AuthorizationToken);
        }

        return client;
    }
}
