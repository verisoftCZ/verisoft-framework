using Verisoft.Core.Common.Store;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Repositories;

public interface IDocumentRepository : IRepository<DocumentEntity, int>
{
    Task<DocumentEntity> GetDocumentWithBlobContent(int id);
}
