using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.AspNet.Authentication;

public class HttpUserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

    public IPrincipal User => contextAccessor.HttpContext?.User;

    public string GetToken()
    {
        if (contextAccessor.HttpContext != null
            && contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationValues))
        {
            var token = authorizationValues[0];
            if (token == null)
            {
                return null;
            }

            token = token.Replace("Bearer ", string.Empty);
            return token;
        }

        return null;
    }
}