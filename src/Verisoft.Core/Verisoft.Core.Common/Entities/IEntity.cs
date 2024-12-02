namespace Verisoft.Core.Common.Entities;

public interface IEntity
{
    object Id { get; set; }

    bool IsNew { get; }
}

public interface IEntity<T> : IEntity
{
    new T Id { get; set; }
}