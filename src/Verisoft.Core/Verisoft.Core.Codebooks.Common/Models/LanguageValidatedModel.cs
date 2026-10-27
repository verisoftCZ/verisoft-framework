using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Codebooks.Common.Models;

public class LanguageValidatedModel : LanguageEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}