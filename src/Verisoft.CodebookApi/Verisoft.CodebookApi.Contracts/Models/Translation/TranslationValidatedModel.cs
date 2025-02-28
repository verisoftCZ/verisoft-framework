using Verisoft.Core.Contracts.Validation;

namespace Verisoft.CodebookApi.Contracts.Models.Translation;

public class TranslationValidatedModel : Translation, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}
