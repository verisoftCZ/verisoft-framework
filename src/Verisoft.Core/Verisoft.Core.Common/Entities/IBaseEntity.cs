using System;

namespace Verisoft.Core.Common.Entities;

public interface IBaseEntity<TKey> : IEntity<TKey>
{
    string CreatedBy { get; set; }

    DateTime CreatedAt { get; set; }

    string UpdatedBy { get; set; }

    DateTime? UpdatedAt { get; set; }

    bool IsDeleted { get; set; }
}
