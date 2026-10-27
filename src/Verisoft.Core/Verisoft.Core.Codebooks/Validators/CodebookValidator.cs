using FluentValidation;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Validation.Validators;

namespace Verisoft.Core.Codebooks.Validators;

public class CodebookValidator : AbstractValidator<CodebookValidatedModel>
{
    public CodebookValidator(ICodebookRepository codebookRepository)
    {
        RuleFor(x => x)
            .SetValidator(new NullInputValidator<CodebookValidatedModel>())
            .DependentRules(() =>
            {
                RuleFor(_ => Id)
                    .HasExistingEntity(codebookRepository)
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Name)
                            .NotEmpty()
                            .WithErrorCode(nameof(ValidationConstants.FieldRequired))
                            .WithMessage(ValidationConstants.FieldRequired)
                            .DependentRules(() =>
                            {
                                RuleFor(x => x.Name)
                                    .MustAsync(async (name, cancellationToken) =>
                                    {
                                        var codebookEntity = await codebookRepository.GetCodebookByName(name);
                                        return Id.HasValue ? codebookEntity is not null && codebookEntity.Id == Id : codebookEntity is null;
                                    })
                                    .WithErrorCode(nameof(ValidationConstants.ValueNotUnique))
                                    .WithMessage(ValidationConstants.ValueNotUnique);
                            });
                    });
            });
    }

    public int? Id { get; set; }
}