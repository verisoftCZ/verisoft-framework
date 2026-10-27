using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueDetailListData<TAdditionalProperties> : PagedData<StringValueDetail<TAdditionalProperties>>
        where TAdditionalProperties : class, new();
