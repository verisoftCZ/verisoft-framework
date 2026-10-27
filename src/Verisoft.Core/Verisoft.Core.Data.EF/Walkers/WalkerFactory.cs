using Microsoft.Extensions.Logging;
using System;

namespace Verisoft.Core.Data.EntityFramework.Walkers;

public class WalkerFactory(ILogger<CascadeWalker> logger) : IWalkerFactory
{
    private readonly ILogger<CascadeWalker> logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public CascadeWalker CreateCascadeWalker(EntityFrameworkDbContextBase context)
    {
        return new CascadeWalker(context, logger);
    }
}
