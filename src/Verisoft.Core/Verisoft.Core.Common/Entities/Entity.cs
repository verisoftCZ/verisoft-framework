namespace Verisoft.Core.Common.Entities;

public class Entity<T> : IEntity<T>
{
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public virtual T? Id { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public bool IsNew => Id.Equals(default(T));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

    object IEntity.Id
    {
#pragma warning disable CS8603 // Possible null reference return.
        get => Id;
#pragma warning restore CS8603 // Possible null reference return.
        set => Id = (T)value;
    }
}
