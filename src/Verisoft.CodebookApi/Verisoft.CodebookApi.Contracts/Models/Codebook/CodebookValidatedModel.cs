using Verisoft.Core.Contracts.Validation;

namespace Verisoft.CodebookApi.Contracts.Models.Codebook;

public class CodebookValidatedModel : CodebookEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}