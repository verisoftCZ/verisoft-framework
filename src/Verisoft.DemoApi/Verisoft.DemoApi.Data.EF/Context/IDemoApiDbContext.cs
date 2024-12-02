using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Common.Store;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Data.EF.Context;

public interface IDemoApiDbContext : IUnitOfWork, IMigrable
{
    public DbSet<ClientEntity> Client { get; set; }

    public DbSet<UserEntity> User { get; set; }

    public DbSet<DocumentEntity> Document { get; set; }

    public DbSet<BlobEntity> Blob { get; set; }
}