using Hangfire.Logging;
using Microsoft.Extensions.Logging;

namespace Verisoft.Core.HangFire
{
    internal class VerisoftHangfireLogProvider : ILogProvider
    {
        private readonly ILogger<VerisoftHangfireLogProvider> logger;

        public VerisoftHangfireLogProvider(ILogger<VerisoftHangfireLogProvider> logger)
        {
            this.logger = logger;
        }

        public ILog GetLogger(string name)
        {
            return new AspNetCoreLog(logger);
        }
    }

}