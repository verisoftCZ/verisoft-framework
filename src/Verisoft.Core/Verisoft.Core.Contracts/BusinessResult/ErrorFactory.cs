using System;
using System.Collections.Generic;
using System.Linq;
using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Contracts.BusinessResult;

public partial class ErrorFactory
{
    public const string NotFoundErrorCode = "NotFound";
    public const string BadRequestErrorCode = "BadRequest";
    public const string NullInputErrorCode = "NullInput";
    public const string UnprocessableEntityErrorCode = "UnprocessableEntity";

    public const string NotFoundErrorMessage = "{entityTypeName} not found by {propertyName}: {propertyValue}";
    public const string NullInputErrorMessage = "The input {inputName} cannot be null.";

    private const string UnprocessableValidatedErrorCode = "UnprocessableValidated";

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was not found.</param>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound(string entityTypeName, string propertyName, string propertyValue)
    {
        return Error(NotFoundErrorCode, NotFoundErrorMessage, new { entityTypeName, propertyName, propertyValue });
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was not found.</param>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound(string entityTypeName, string propertyName, int propertyValue) => NotFound(entityTypeName, propertyName, propertyValue.ToString());

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was not found.</param>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound(string entityTypeName, string propertyName, Guid propertyValue) => NotFound(entityTypeName, propertyName, propertyValue.ToString());

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <typeparam name="TEntityType">Type of the entity that was not found.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound<TEntityType>(string propertyName, string propertyValue) => NotFound(typeof(TEntityType).Name, propertyName, propertyValue);

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <typeparam name="TEntityType">Type of the entity that was not found.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound<TEntityType>(string propertyName, int propertyValue) => NotFound(typeof(TEntityType).Name, propertyName, propertyValue);

    /// <summary>
    /// Creates a BusinessActionError object that signifies an entity of a specified type was not found.
    /// This method supports API endpoints in returning a 404 Not Found Object Result when an entity
    /// specified by the client cannot be located in the system.
    /// </summary>
    /// <typeparam name="TEntityType">Type of the entity that was not found.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used in the attempt to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used in the attempt to retrieve the entity.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used to search for it.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the absence of a requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 404 HTTP response.
    /// </remarks>
    public static BusinessActionError NotFound<TEntityType>(string propertyName, Guid propertyValue) => NotFound(typeof(TEntityType).Name, propertyName, propertyValue);

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the request was malformed.
    /// This method supports API endpoints in returning a 400 Bad Request Object Result.
    /// </summary>
    /// <param name="message">The specific error message detailing why the request was bad.</param>"
    /// <returns>
    /// BusinessActionError object populated with error code and message.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the malformed request.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 400 HTTP response.
    /// </remarks>
    public static BusinessActionError BadRequest(string message)
    {
        return Error(BadRequestErrorCode, message, new { message });
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the input of request was not provided.
    /// This method supports API endpoints in returning a 400 Bad Request Object Result.
    /// </summary>
    /// <param name="inputName">Name of crucial method parameter that was null.</param>"
    /// <returns>
    /// BusinessActionError object populated with error code and message.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the malformed request.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 400 HTTP response.
    /// </remarks>
    public static BusinessActionError NullInput(string inputName)
    {
        return Error(NullInputErrorCode, NullInputErrorMessage, new { inputName });
    }

    public static BusinessActionErrors UnprocessableEntity(List<ValidationProblem> validatedObjectValidationProblems)
    {
        var errors = new BusinessActionErrors();
        errors.AddRange(validatedObjectValidationProblems
            .Select(problems => new BusinessActionError(problems.Code, problems.Message, problems.PropertyName)));

        return errors;
    }

    public static BusinessActionError UnprocessableEntity(string message)
    {
        return Error(UnprocessableEntityErrorCode, message);
    }

    public static BusinessActionError UnprocessableEntity(string message, object parameters)
    {
        return Error(UnprocessableEntityErrorCode, message, parameters);
    }

    internal static BusinessActionError UnprocessableValidated()
    {
        return Error(UnprocessableValidatedErrorCode, "There are validation errors.");
    }

    protected static BusinessActionError Error(string code, string message, object parameters = null)
    {
        return new BusinessActionError(code, message, parameters);
    }
}