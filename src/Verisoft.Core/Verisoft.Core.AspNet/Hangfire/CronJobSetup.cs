using Hangfire;
using Hangfire.Common;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Verisoft.Core.AspNet.Hangfire
{
    public static class CronJobSetup
    {
        public static void AddCronJob<T>(Expression<Func<T, Task>> methodCall, bool isEnabled, string cronExpression)
        {
            string jobId = GetRecurringJobId(methodCall) ?? throw new NullReferenceException("Recurrent JobId is null");

            if (isEnabled)
            {
                RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists(jobId);
            }
        }

        public static void AddCronJob<T>(Expression<Func<T, Task>> methodCall, BaseCronConfig baseCronConfig)
        {
            AddCronJob<T>(methodCall, baseCronConfig.CronEnabled, baseCronConfig.CronSettings);
        }

        public static void AddCronJob<T>(Expression<Func<T, Task>> methodCall, Action<BaseCronConfig> options)
        {
            var config = new BaseCronConfig();
            options?.Invoke(config);
            AddCronJob<T>(methodCall, config.CronEnabled, config.CronSettings);
        }

        private static string GetRecurringJobId<T>(Expression<Func<T, Task>> methodCall)
        {
            var job = Job.FromExpression(methodCall);
            if (IsValidRecurrentJobId(job))
            {
                return $"{job.Type.Name}.{job.Method.Name}";
            }

            return null;
        }

        private static bool IsValidRecurrentJobId(Job job)
        {
            return job != null && job.Method != null && job.Type != null && !string.IsNullOrEmpty(job.Type.Name) && !string.IsNullOrEmpty(job.Method.Name);
        }
    }
}
