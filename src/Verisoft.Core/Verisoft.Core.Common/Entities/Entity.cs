namespace Verisoft.Core.Common.Entities;

public class Entity<T> : IEntity<T>
{
    public virtual T Id { get; set; }

    public bool IsNew => Id.Equals(default(T));

    object IEntity.Id
    {
        get => Id;
        set => Id = (T)value;
    }
}
