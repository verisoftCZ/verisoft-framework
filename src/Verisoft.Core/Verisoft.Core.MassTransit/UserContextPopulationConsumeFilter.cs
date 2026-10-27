using MassTransit;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.MassTransit;

public class UserContextPopulationConsumeFilter<T>(IUserContext userContext)
    : IFilter<ConsumeContext<T>>
    where T : class
{
    private const string FilterType = "userContextPopulation";
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(FilterType);
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        if (context.Headers.TryGetHeader(HeaderConstants.AuthorizationHeader, out var authorizationHeader)
            && authorizationHeader is string authorizationHeaderString
            && authorizationHeaderString.StartsWith(HeaderConstants.AuthorizationTokenBearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeaderString[HeaderConstants.AuthorizationTokenBearerPrefix.Length..];
            if (!string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var identity = new System.Security.Claims.ClaimsIdentity(jwtToken.Claims, HeaderConstants.BearerAuthenticationType);
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                    userContext.SetUser(principal);
                    userContext.SetToken(token);
                }
            }
        }
        else
        {
            userContext.ClearInjectedUserAndToken();
        }

        await next.Send(context);
    }
}