using Verisoft.CodebookApi.Contracts.Models.Base;

namespace Verisoft.CodebookApi.Application.Helpers;

public static class ObjectReflectionHelper
{
    public static bool ContainsAdditionalTenantProperties(object obj)
    {
        return obj is not null && obj.GetType()
                                     .GetProperties()
                                     .Any(p => p.Name is not nameof(ITenantValue.TenantId)
                                                      and not nameof(ITenantValue.IsForbidden));
    }
}
