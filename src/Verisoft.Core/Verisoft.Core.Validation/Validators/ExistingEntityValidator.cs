using FluentValidation;
using FluentValidation.Validators;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Validation.Validators;

public class ExistingEntityValidator<TValidatedObject, TEntity, TKey>(IRepository<TEntity, TKey> repository)
    : AsyncPropertyValidator<TValidatedObject, TKey>
    where TValidatedObject : IValidatedObject
    where TEntity : IEntity<TKey>
{
    private const string IdPropertyName = "Id";
    private readonly IRepository<TEntity, TKey> repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public override string Name => "RemoveFromErrors";

    public override async Task<bool> IsValidAsync(ValidationContext<TValidatedObject> context, TKey value, CancellationToken cancellation)
    {
        var entity = await repository.GetAsync(value);
        if (entity is not null)
        {
            return true;
        }

        context.AddFailure(new FluentValidation.Results.ValidationFailure
        {
            PropertyName = IdPropertyName,
            ErrorMessage = ErrorFactory.NotFoundErrorMessage,
            ErrorCode = ErrorFactory.NotFoundErrorCode,
            AttemptedValue = new { entityTypeName = typeof(TEntity).Name, propertyName = nameof(IEntity.Id), propertyValue = value, },
        });

        return false;

    }
}