using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Contracts.Filters;

public sealed class UserFilter
{
    public string[] FirstName { get; set; }

    public string[] Surname { get; set; }

    public string[] Email { get; set; }

    public DocumentEntity[] Documents { get; set; }

    public string SearchField { get; set; }
}
