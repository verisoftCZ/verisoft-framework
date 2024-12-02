using System.Collections.Generic;

namespace Verisoft.Core.Contracts
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ValidationProblemDetails
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int? Status { get; set; }

        public string Detail { get; set; }

        public string Instance { get; set; }

        public IDictionary<string, ICollection<string>> Errors { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    }
}
