namespace Verisoft.Core.Common.Entities;

public interface IOptimisticLockable
{
    byte[] Version { get; set; }
}
