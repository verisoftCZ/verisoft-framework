using System;

namespace Verisoft.Core.Contracts.BusinessResult;

public partial class ErrorFactory
{
    public const string ForbiddenErrorCode = "Forbidden";

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
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(string userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return Error(
            ForbiddenErrorCode,
            "Access to is forbidden to user {userIdentificator}. For access the user needs {permissionCheckMode} of the following permissions: {requiredPermissions}",
            new
            {
                userIdentificator,
                permissionCheckMode = permissionCheckMode.ToString(),
                requiredPermissions,
            });
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(int userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return Forbidden(userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
    }

    /// <summary>
    /// Creates a BusinessActionError object that signifies that the client does not have permission for action.
    /// This method supports API endpoints in returning a 403 Forbidden Object Result when an action
    /// is forbidden to the client due to insufficient permissions.
    /// </summary>
    /// <param name="userIdentificator">The identifier of the user that has insufficient permissions.</param>
    /// <param name="permissionCheckMode">The method used when checking user permissions.</param>
    /// <param name="requiredPermissions">The required permissions for access.</param>
    /// <returns>
    /// BusinessActionError object populated with error code, error message and with user identification.
    /// </returns>
    /// <remarks>
    /// This method should be used within methods returning BusinessActionResult objects to inform the client about the insufficient permission.
    /// When used with extension method ToActionResult(), provided parameters are included in the body of the 403 HTTP response.
    /// </remarks>
    public static BusinessActionError Forbidden(Guid userIdentificator, PermissionCheckMode permissionCheckMode, params string[] requiredPermissions)
    {
        return Forbidden(userIdentificator.ToString(), permissionCheckMode, requiredPermissions);
    }
}