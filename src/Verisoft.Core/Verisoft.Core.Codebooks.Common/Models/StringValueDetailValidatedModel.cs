using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueDetailValidatedModel<TAdditionalProperties> : StringValueDetailEditModel<TAdditionalProperties>, IValidatedObject
    where TAdditionalProperties : class
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}