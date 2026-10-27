namespace Verisoft.Core.Contracts;

/// <summary>
/// An enum to specify how permissions should be checked.
/// </summary>
public enum PermissionCheckMode
{
    /// <summary>
    /// At least one specified permissions is required.
    /// </summary>
    Any,

    /// <summary>
    /// All specified permissions are required.
    /// </summary>
    All,
}