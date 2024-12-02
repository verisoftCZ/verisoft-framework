using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class ClientRepository(IDemoApiDbContext unitOfWork, IUserContext userContext, ILogger<ClientRepository> logger)
    : BaseRepository<ClientEntity, int>(unitOfWork, userContext, logger), IClientRepository
{
    protected override DbSet<ClientEntity> GetDbSet()
    {
        return Context.Client;
    }
}
