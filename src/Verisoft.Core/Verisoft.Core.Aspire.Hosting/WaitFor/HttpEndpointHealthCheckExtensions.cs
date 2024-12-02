/*
MIT License
Copyright (c) 2024 David Fowler

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Original source: https://github.com/davidfowl/WaitForDependenciesAspire
*/

using Aspire.Hosting.ApplicationModel;
using HealthChecks.Uris;

namespace Verisoft.Core.Aspire.Hosting.WaitFor;

public static class HttpEndpointHealthCheckExtensions
{
    /// <summary>
    /// Adds a health check to the resource with HTTP endpoints.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="endpointName">The optional name of the endpoint. If not specified, will be the first http or https endpoint (based on scheme).</param>
    /// <param name="path">path to send the HTTP request to. This will be appended to the base URL of the resolved endpoint.</param>
    /// <param name="configure">A callback to configure the options for this health check.</param>
    public static IResourceBuilder<T> WithHealthCheck<T>(
        this IResourceBuilder<T> builder,
        string? endpointName = null,
        string path = "health",
        Action<UriHealthCheckOptions>? configure = null)
        where T : IResourceWithEndpoints
    {
        return builder.WithAnnotation(new HealthCheckAnnotation(async (resource, ct) =>
        {
            if (resource is not IResourceWithEndpoints resourceWithEndpoints)
            {
                return null;
            }

            var endpoint = endpointName is null
                ? resourceWithEndpoints.GetEndpoints().FirstOrDefault(e => e.Scheme is "http" or "https")
                : resourceWithEndpoints.GetEndpoint(endpointName);

            var url = endpoint?.Url;

            if (url is null)
            {
                return null;
            }

            var options = new UriHealthCheckOptions();

            options.AddUri(new(new(url), path));

            configure?.Invoke(options);

            var client = new HttpClient();
            return await Task.FromResult(new UriHealthCheck(options, () => client));
        }));
    }
}