namespace Verisoft.DemoApi.Host.HangfireJobs
{
    public class HangfireJobsConfig
    {
        public HangfireJobsConfig(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.GetSection("HangfireJobs").Bind(this);
        }

        public string JobOneSettings { get; set; }

        public string JobTwoSettings { get; set; }
    }
}
