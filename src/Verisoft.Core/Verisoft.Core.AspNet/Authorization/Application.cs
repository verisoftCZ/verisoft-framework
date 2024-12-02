using System.Collections.Generic;

namespace Verisoft.Core.AspNet.Authorization
{
    public class Application
    {
        public string ApplicationName { get; set; }

        public string Key { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
