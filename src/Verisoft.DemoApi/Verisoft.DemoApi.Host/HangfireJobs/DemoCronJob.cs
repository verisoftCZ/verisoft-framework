namespace Verisoft.DemoApi.Host.HangfireJobs
{
    public class DemoCronJob
    {
        private readonly ILogger<DemoCronJob> logger;

        public DemoCronJob(ILogger<DemoCronJob> logger)
        {
            this.logger = logger;
        }

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
}
