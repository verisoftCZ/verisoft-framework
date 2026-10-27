namespace Verisoft.Core.HangFire.Config;

public class BaseCronConfig
{
    public bool CronEnabled { get; set; }

    public string CronSettings { get; set; } = string.Empty;
}
