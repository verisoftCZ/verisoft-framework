using System.Collections.Generic;

namespace Verisoft.Core.AspNet.Authorization
{
    public interface IAuthConfig
    {
        IEnumerable<Application> Applications { get; }
    }
}
