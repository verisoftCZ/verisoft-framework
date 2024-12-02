using System;
using System.ComponentModel.DataAnnotations;

namespace Verisoft.Core.Common.Entities;

public class BaseEntity<TKey> : Entity<TKey>, IBaseEntity<TKey>
{
    [Required]
    [StringLength(255)]
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    [StringLength(255)]
    public string UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
