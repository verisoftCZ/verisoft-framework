using Verisoft.CodebookApi.Core.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class MultiTenantRepository : ITenantRepository
{
    public bool IsTenantEnvironment => true;

    public Task<List<int>> GetAllTenantIdsAsync()
    {
        throw new NotImplementedException();
    }
}