using System;

namespace Verisoft.Core.Contracts.BusinessResult;

public class ErrorFactory
{
    public const string NotFoundErrorCode = "NotFound";
    public const string ForbiddenErrorCode = "Forbidden";
    public const string BadRequestErrorCode = "BadRequest";
    public const string NullInputErrorCode = "NullInput";
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
        return Error(NotFoundErrorCode, "{entityTypeName} not found by {propertyName}: {propertyValue}", new { entityTypeName, propertyName, propertyValue });
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
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, string userIdentificator)
    {
        return Error(ForbiddenErrorCode, "Access to resource {entityTypeName} with {propertyName}: {propertyValue} is forbidden to user {userIdentificator}", new { entityTypeName, propertyName, propertyValue, userIdentificator });
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, string userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, string userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, int userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue, userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, int userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, int userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue, userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, string userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, string userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, string userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, int userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, int userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, int userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, Guid userIdentificator)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(string userIdentificator)
    {
        return Error(ForbiddenErrorCode, "Access to is forbidden to user {userIdentificator}", new { userIdentificator });
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(int userIdentificator)
    {
        return Forbidden(userIdentificator.ToString());
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(Guid userIdentificator)
    {
        return Forbidden(userIdentificator.ToString());
    }

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
        return Error(BadRequestErrorCode, "The input {inputName} cannot be null.", new { inputName });
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
