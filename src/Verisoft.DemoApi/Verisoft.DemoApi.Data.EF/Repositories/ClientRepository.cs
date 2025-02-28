using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Repositories;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class ClientRepository(IDemoApiDbContext unitOfWork, IUserContext userContext)
    : HistoryRepositoryBase<ClientEntity, int>(unitOfWork, userContext), IClientRepository
{
    public async Task<ClientEntity> FindByTradeIdAsync(string tradeId, CancellationToken cancellationToken)
    {
        return await GetDbSet()
            .FirstOrDefaultAsync(c => c.TradeId == tradeId, cancellationToken);
    }

    public async Task<ClientEntity> FindByVatIdAsync(string vatId, CancellationToken cancellationToken)
    {
        return await GetDbSet()
            .FirstOrDefaultAsync(c => c.VatId == vatId, cancellationToken);
    }

    protected override DbSet<ClientEntity> GetDbSet() => ((IDemoApiDbContext)UnitOfWork).Client;
}