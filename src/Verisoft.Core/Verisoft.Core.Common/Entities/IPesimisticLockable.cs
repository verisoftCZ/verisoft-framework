using System;

namespace Verisoft.Core.Common.Entities;

public interface IPesimisticLockable
{
    public string LockedBy { get; set; }

    public DateTime? LockedAt { get; set; }
}
