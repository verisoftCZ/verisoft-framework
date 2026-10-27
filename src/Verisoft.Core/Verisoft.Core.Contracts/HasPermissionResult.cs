using System.Diagnostics.CodeAnalysis;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Contracts;

public readonly struct HasPermissionResult
{
    [SetsRequiredMembers]
    public HasPermissionResult()
    {
        HasPermission = true;
        Error = null;
    }

    [SetsRequiredMembers]
    public HasPermissionResult(BusinessActionError error)
    {
        HasPermission = false;
        Error = error;
    }

    [MemberNotNullWhen(false, nameof(Error))]
    public required bool HasPermission { get; init; }

    public BusinessActionError? Error { get; init; }

    public static implicit operator HasPermissionResult(BusinessActionError error)
        => new(error);
}
