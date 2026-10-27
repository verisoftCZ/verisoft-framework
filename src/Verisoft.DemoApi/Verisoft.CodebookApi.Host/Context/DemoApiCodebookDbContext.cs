using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Constants;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Context;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.CodebookApi.Host.Context;

public class DemoApiCodebookDbContext : CodebookDbContext, IDemoApiCodebookDbContext
{
    private readonly IDatabaseConfig databaseConfig;

    public DemoApiCodebookDbContext(
        IDatabaseConfig databaseConfig,
        ICodebookRegistry codebookRegistry)
        : base(databaseConfig, codebookRegistry)
    {
        this.databaseConfig = databaseConfig;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                databaseConfig.ConnectionString,
                x => x.MigrationsHistoryTable("__MigrationsHistory", SchemaConstants.CodebookSchema));
        }
    }
}