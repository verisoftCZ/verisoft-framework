using System.Collections.Generic;

namespace Verisoft.Core.Contracts.Validation;

public interface IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; }
}
