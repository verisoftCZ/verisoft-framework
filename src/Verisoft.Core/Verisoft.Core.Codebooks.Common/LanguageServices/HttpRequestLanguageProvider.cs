using Microsoft.AspNetCore.Http;

namespace Verisoft.Core.Codebooks.Common.LanguageServices;

public class HttpRequestLanguageProvider(IHttpContextAccessor httpContextAccessor) : ILanguageProvider
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public string? PreferredLanguageCode
    {
        get
        {
            return httpContextAccessor.HttpContext?.Request.Headers.AcceptLanguage.FirstOrDefault();
        }
    }
}