using System;

namespace Verisoft.Core.Common.Exceptions;

public class InvalidSortPropertyException : Exception
{
    public InvalidSortPropertyException(
        Type entityType,
        string? propertyName)
        : base(CreateMessage(entityType, propertyName))
    {
        EntityType = entityType;
        PropertyName = propertyName;
    }

    public InvalidSortPropertyException()
    {
    }

    public InvalidSortPropertyException(string? message)
        : base(message)
    {
    }

    public InvalidSortPropertyException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public Type? EntityType { get; }

    public string? PropertyName { get; }

    private static string CreateMessage(Type entityType, string? propertyName)
        => $"The property '{propertyName}' does not exist or cannot be used for sorting on entity type '{entityType.Name}'.";
}