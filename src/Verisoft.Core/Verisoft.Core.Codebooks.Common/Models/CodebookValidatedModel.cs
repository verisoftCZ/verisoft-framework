using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Codebooks.Common.Models;

public class CodebookValidatedModel : CodebookEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}