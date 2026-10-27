using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace Verisoft.Core.HangFire.Filter;

public class AllowAccesFilter(IEnumerable<string> roles) : IDashboardAuthorizationFilter
{
    private readonly IEnumerable<string> roles = roles ?? throw new ArgumentException("hangfire dashboard roles can't be null");

    public bool Authorize([NotNull] DashboardContext context)
    {
        if (roles.Any())
        {
            var httpContext = context.GetHttpContext();
            return roles.Any(role => httpContext.User.IsInRole(role));
        }

        return false;
    }
}
