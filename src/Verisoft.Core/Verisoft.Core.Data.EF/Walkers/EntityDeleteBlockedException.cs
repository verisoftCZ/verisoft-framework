using System;
using System.Collections.Generic;

namespace Verisoft.Core.Data.EntityFramework.Walkers;

public class EntityDeleteBlockedException : Exception
{
    public EntityDeleteBlockedException(
        Type entityType,
        object? entityId,
        IReadOnlyCollection<string> violations)
        : base(CreateMessage(entityType, entityId, violations))
    {
        EntityType = entityType;
        EntityId = entityId;
        Violations = violations;
    }

    public EntityDeleteBlockedException()
    {
    }

    public EntityDeleteBlockedException(string? message)
        : base(message)
    {
    }

    public EntityDeleteBlockedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public Type? EntityType { get; }

    public object? EntityId { get; }

    public IReadOnlyCollection<string> Violations { get; } = [];

    private static string CreateMessage(
        Type entityType,
        object? entityId,
        IReadOnlyCollection<string> violations)
    {
        return $"Entity {entityType.Name} (ID: {entityId}) cannot be deleted because it is still referenced: {string.Join("; ", violations)}";
    }
}