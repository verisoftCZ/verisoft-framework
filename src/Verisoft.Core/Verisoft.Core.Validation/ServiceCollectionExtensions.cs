using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Validation.Services;

namespace Verisoft.Core.Validation;

public static class ServiceCollectionExtensions
{
    public static void AddVerisoftFluentValidation<TValidator>(this IServiceCollection serviceCollection)
        where TValidator : IValidator
    {
        serviceCollection.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        serviceCollection.AddValidatorsFromAssemblyContaining<TValidator>();
        serviceCollection.AddScoped<IValidationService, ValidationService>();
    }
}
