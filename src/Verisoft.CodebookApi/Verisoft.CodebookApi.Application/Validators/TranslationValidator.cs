using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Contracts.Models.Translation;

namespace Verisoft.CodebookApi.Application.Validators;

public class TranslationValidator : AbstractValidator<TranslationValidatedModel>
{
    public TranslationValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage(CodebookApiErrorFactory.TranslationValueNotSetErrorCode)
            .WithErrorCode(CodebookApiErrorFactory.TranslationValueNotSetErrorMessage);
    }
}