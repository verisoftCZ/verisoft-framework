namespace Verisoft.DemoApi.Host.HangfireJobs;

public class DemoCronJob(ILogger<DemoCronJob> logger)
{
    public async Task MakeSomeCronJob()
    {
        logger.LogInformation("Starting CRON job");
        await Task.Delay(1000);
        logger.LogInformation("Finishing CRON job");
    }

    public async Task MakeSomeCronJobTwo()
    {
        logger.LogInformation("Starting CRON job");
        await Task.Delay(1000);
        logger.LogInformation("Finishing CRON job");
    }
}