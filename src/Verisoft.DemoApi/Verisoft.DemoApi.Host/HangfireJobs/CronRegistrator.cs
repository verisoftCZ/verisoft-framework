using Verisoft.Core.HangFire;

namespace Verisoft.DemoApi.Host.HangfireJobs
{
    public static class CronRegistrator
    {
        public static void RegisterCronJobs(this IApplicationBuilder app)
        {
            var hangfireJobsConfig = app.ApplicationServices.GetService<HangfireJobsConfig>();
            if (hangfireJobsConfig != null && hangfireJobsConfig.JobOneSettings != null)
            {
                CronJobSetup.AddCronJob<DemoCronJob>(
                    (x) => x.MakeSomeCronJob(),
                    options =>
                    {
                        options.CronEnabled = false;
                        options.CronSettings = hangfireJobsConfig.JobOneSettings;
                    });

                CronJobSetup.AddCronJob<DemoCronJob>(
                   (x) => x.MakeSomeCronJobTwo(),
                   options =>
                   {
                       options.CronEnabled = true;
                       options.CronSettings = hangfireJobsConfig.JobOneSettings;
                   });
            }
        }
    }
}
