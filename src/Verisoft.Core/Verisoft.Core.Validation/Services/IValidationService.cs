using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Validation.Services;

public interface IValidationService
{
    Task<TValidatedObject> ValidateAsync<TValidatedObject, TSource>(TSource sourceObject, object id = null)
        where TValidatedObject : class, IValidatedObject
        where TSource : class;
}
