using Verisoft.CodebookApi.Core.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class SingleTenantRepository : ITenantRepository
{
    private const int MockTenantIdForNonTenantEnvironment = 1;

    public bool IsTenantEnvironment => false;

    public Task<List<int>> GetAllTenantIdsAsync() => Task.FromResult(new List<int> { MockTenantIdForNonTenantEnvironment });
}