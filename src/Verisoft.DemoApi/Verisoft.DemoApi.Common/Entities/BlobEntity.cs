using Verisoft.Core.Common.Entities;

namespace Verisoft.DemoApi.Common.Entities;

public class BlobEntity : BaseEntity<int>
{
    public byte[] Content { get; set; }

    public DocumentEntity Document { get; set; }

    public int DocumentId { get; set; }
}
