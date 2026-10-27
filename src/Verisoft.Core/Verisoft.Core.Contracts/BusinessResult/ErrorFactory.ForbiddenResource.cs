using System;

namespace Verisoft.Core.Contracts.BusinessResult;

public partial class ErrorFactory
{
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
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="entityTypeName">The name of Type of the entity that was requested.</param>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return Error(
            ForbiddenErrorCode,
            "Access to resource {entityTypeName} with {propertyName}: {propertyValue} is forbidden to user {userIdentificator}. To access this resource the user needs {permissionCheckMode} of the following permissions: {requiredPermissions}",
            new
            {
                entityTypeName,
                propertyName,
                propertyValue,
                userIdentificator,
                permissionCheckMode = permissionCheckMode.ToString(),
                requiredPermissions,
            });
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator, permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator, permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue, userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, string propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue, userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, int propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource(string entityTypeName, string propertyName, Guid propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(entityTypeName, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
    }

    // Generic overloads

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission to access the requested resource.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an entity
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that was requested.</typeparam>
    /// <param name="propertyName">A string representation of name of parameters used to retrieve the entity.</param>
    /// <param name="propertyValue">The value of the parameter used to retrieve the entity.</param>
    /// <param name="userIdentificator">The identifier of the user that attempted to access the resource.</param>
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator, permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator, permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator, permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, string propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue, userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, int propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
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
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and the parameters used retrieve resource along with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission for requested resource.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError ForbiddenResource<TEntity>(string propertyName, Guid propertyValue, Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return ForbiddenResource(typeof(TEntity).Name, propertyName, propertyValue.ToString(), userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
    }
}