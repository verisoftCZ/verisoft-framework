using System.ComponentModel.DataAnnotations;
using Verisoft.Core.Common.Entities;

namespace Verisoft.DemoApi.Common.Entities;

public class UserEntity : BaseEntity<int>
{
    [Required]
    [MaxLength(255)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Surname { get; set; }

    [Required]
    [MaxLength(512)]
    public required string Email { get; set; }

    [MaxLength(512)]
    public required string ApiKey { get; set; }

    [Required]
    [MaxLength(1024)]
    public required string PasswordHash { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Salt { get; set; }

    public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();
}
