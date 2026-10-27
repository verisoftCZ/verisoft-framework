using Microsoft.Extensions.Configuration;

namespace Verisoft.Core.HangFire.Config;

public class HangfireDashboardConfig
{
    public HangfireDashboardConfig(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        configuration.GetSection("HangfireDashboardConfig").Bind(this);
    }

    public string? AppPath { get; set; } = null;

    public string? PrefixPath { get; set; } = null;

    public bool AuthorizationEnabled { get; set; } = true;

    public string[] AllowedRoles { get; set; } = ["Admin"];
}
