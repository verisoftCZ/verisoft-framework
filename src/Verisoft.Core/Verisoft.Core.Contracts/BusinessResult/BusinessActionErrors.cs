using System.Collections.Generic;

namespace Verisoft.Core.Contracts.BusinessResult;

public class BusinessActionErrors : List<BusinessActionError>
{
    public bool HasErrors => Count > 0;

    public static implicit operator BusinessActionErrors(BusinessActionError error)
    {
        return [error];
    }

    public static BusinessActionErrors FromList(IEnumerable<BusinessActionError> errors)
    {
        return [.. errors];
    }
}
