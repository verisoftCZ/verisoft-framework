using System.Collections.Generic;

namespace Verisoft.Core.Contracts.Validations;

public interface IValidatedObject
{
    public List<ValidationError> ValidationErrors { get; set; }

    public List<ValidationWarning> ValidationWarnings { get; set; }
}
