using Microsoft.EntityFrameworkCore;

namespace Verisoft.CodebookApi.Database.Configurations;

public interface ICodebookRecordSchemaManager
{
    void ConfigureTables(ModelBuilder modelBuilder);
}
