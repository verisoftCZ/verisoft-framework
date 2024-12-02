using System.ComponentModel.DataAnnotations;
using Verisoft.Core.Common.Entities;

namespace Verisoft.DemoApi.Common.Entities;

public class DocumentEntity : BaseEntity<int>
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(4000)]
    public string Description { get; set; }

    public DateTime? ValidTo { get; set; }

    public DateTime? ValidFrom { get; set; }

    public int UserId { get; set; }

    public UserEntity User { get; set; }

    public int? ClientId { get; set; }

    public ClientEntity Client { get; set; }

    public BlobEntity Blob { get; set; }

    [MaxLength(255)]
    public string ContentType { get; set; }
}