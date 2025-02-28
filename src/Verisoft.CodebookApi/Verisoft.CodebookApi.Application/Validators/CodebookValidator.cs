using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Contracts.Models.Codebook;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Validation.Validators;

namespace Verisoft.CodebookApi.Application.Validators;

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
                            .WithErrorCode(CodebookApiErrorFactory.CodebookNameNotSetErrorCode)
                            .WithMessage(CodebookApiErrorFactory.CodebookNameNotSetErrorMessage)
                            .DependentRules(() =>
                            {
                                RuleFor(x => x.Name)
                                    .MustAsync(async (name, cancellationToken) =>
                                    {
                                        var codebookEntity = await codebookRepository.GetCodebookByName(name);
                                        return Id.HasValue ? codebookEntity is null || codebookEntity.Id == Id : codebookEntity is null;
                                    })
                                    .WithErrorCode(CodebookApiErrorFactory.CodebookAlreadyExistErrorCode)
                                    .WithMessage(CodebookApiErrorFactory.CodebookAlreadyExistErrorMessage);
                            });
                    });
            });
    }

    public int? Id { get; set; }
}