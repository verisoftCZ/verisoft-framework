namespace Verisoft.CodebookApi.Core.Repositories;

public interface ITenantRepository
{
    public bool IsTenantEnvironment { get; }

    Task<List<int>> GetAllTenantIdsAsync();
}