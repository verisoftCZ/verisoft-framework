using MassTransit;
using System;
using System.Threading.Tasks;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.MassTransit;

public class AutoAuthorizationHeaderPublishFilter<T>(IUserContext userContext)
    : IFilter<PublishContext<T>>
    where T : class
{
    private const string FilterType = "autoAuthorizationHeader";
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(FilterType);
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var token = userContext.GetToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            context.Headers.Set(HeaderConstants.AuthorizationHeader, string.Concat(HeaderConstants.AuthorizationTokenBearerPrefix, token));
        }

        await next.Send(context);
    }
}