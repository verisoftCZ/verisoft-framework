using Verisoft.Core.Contracts.Validation;

namespace Verisoft.CodebookApi.Contracts.Models.Language;

public class LanguageValidatedModel : LanguageEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}