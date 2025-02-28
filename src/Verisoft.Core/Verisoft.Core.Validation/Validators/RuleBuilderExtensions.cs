using FluentValidation;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Validation.Validators;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<TValidatedObject, TKey?> HasExistingEntity<TValidatedObject, TEntity, TKey>(
        this IRuleBuilder<TValidatedObject, TKey?> ruleBuilder,
        IRepository<TEntity, TKey> repository)
        where TValidatedObject : IValidatedObject
        where TEntity : IEntity<TKey>
        where TKey : struct
    {
        return ruleBuilder
            .SetAsyncValidator(new ExistingEntityValidator<TValidatedObject, TEntity, TKey>(repository));
    }
}