using Verisoft.Core.Common.Store;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Repositories;

public interface IClientRepository : IHistoryRepository<ClientEntity, int>
{
    Task<ClientEntity> FindByTradeIdAsync(string tradeId, CancellationToken cancellationToken);

    Task<ClientEntity> FindByVatIdAsync(string vatId, CancellationToken cancellationToken);
}