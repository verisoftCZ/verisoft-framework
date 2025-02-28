using FluentValidation;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.Validation;
using Verisoft.Core.Contracts.Validation.Extensions;

namespace Verisoft.Core.Validation.Services;

public class ValidationService(ITypeMapper typeMapper, IServiceProvider serviceProvider) : IValidationService
{
    private const string IdPropertyName = "Id";
    private readonly ITypeMapper typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
    private readonly IServiceProvider serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TValidatedObject> ValidateAsync<TValidatedObject, TSource>(TSource sourceObject, object id = null)
        where TValidatedObject : class, IValidatedObject
        where TSource : class
    {
        var validatedObject = typeMapper.Map<TValidatedObject>(sourceObject);
        await ValidateObjectRecursivelyAsync(validatedObject, id);
        return validatedObject;
    }

    private async Task ValidateObjectRecursivelyAsync(IValidatedObject validatedObject, object id = null)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(validatedObject.GetType());
        var validator = serviceProvider.GetService(validatorType) as IValidator;

        if (id is not null)
        {
            var idProperty = validator?.GetType().GetProperty(IdPropertyName);

            if (idProperty is not null && idProperty.PropertyType.IsInstanceOfType(id))
            {
                idProperty.SetValue(validator, id);
            }
        }

        if (validator is not null)
        {
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(validatedObject.GetType());
            var validationContext = Activator.CreateInstance(validationContextType, validatedObject);
            var validateAsyncMethod = validator.GetType().GetMethod(nameof(IValidator.ValidateAsync), [validationContextType, typeof(CancellationToken)]);

            if (validateAsyncMethod is not null)
            {
                var validationResult = await (Task<FluentValidation.Results.ValidationResult>)validateAsyncMethod.Invoke(validator, [validationContext, CancellationToken.None]);
                validationResult.Errors?.ForEach(validatedObject.LogError);
            }
        }

        var properties = validatedObject.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (typeof(IValidatedObject).IsAssignableFrom(property.PropertyType))
            {
                if (property.GetValue(validatedObject) is IValidatedObject nestedObject)
                {
                    await ValidateObjectRecursivelyAsync(nestedObject);
                }
            }
            else if (typeof(IEnumerable<IValidatedObject>).IsAssignableFrom(property.PropertyType) && property.GetValue(validatedObject) is IEnumerable<IValidatedObject> nestedCollection)
            {
                foreach (var item in nestedCollection)
                {
                    await ValidateObjectRecursivelyAsync(item);
                }
            }
        }
    }
}