using Microsoft.AspNetCore.Http;
using System;
using System.Security.Principal;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.AspNet.Authentication;

public class HttpUserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    private static readonly IPrincipal FallbackPrincipal = new GenericPrincipal(new GenericIdentity("System"), []);
    private readonly IHttpContextAccessor contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

    private IPrincipal? injectedUser;

    private string? injectedToken;

    public IPrincipal User => contextAccessor.HttpContext?.User ?? injectedUser ?? FallbackPrincipal;

    public virtual string? GetToken()
    {
        if (contextAccessor.HttpContext != null
            && contextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderConstants.AuthorizationHeader, out var authorizationValues))
        {
            var token = authorizationValues[0];
            if (token == null)
            {
                return null;
            }

            return token.Replace(HeaderConstants.AuthorizationTokenBearerPrefix, string.Empty);
        }

        if (!string.IsNullOrWhiteSpace(injectedToken))
        {
            return injectedToken.Replace(HeaderConstants.AuthorizationTokenBearerPrefix, string.Empty);
        }

        return null;
    }

    public virtual void SetUser(IPrincipal user)
    {
        injectedUser = user ?? throw new ArgumentNullException(nameof(user));
    }

    public virtual void SetToken(string token)
    {
        injectedToken = token ?? throw new ArgumentNullException(nameof(token));
    }

    public void ClearInjectedUserAndToken()
    {
        injectedUser = null;
        injectedToken = null;
    }

    public virtual string GetUserName()
    {
        return User?.Identity?.Name ?? "Unknown";
    }
}