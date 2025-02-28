using Verisoft.Core.Contracts.Validation;

namespace Verisoft.CodebookApi.Contracts.Models.Translation;

public class StringValueTranslationsValidatedModel : IValidatedObject
{
    public IEnumerable<TranslationValidatedModel> Translations { get; set; } = [];

    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}