using System;
using System.Diagnostics.CodeAnalysis;

namespace Verisoft.Core.Common.Entities.Codebooks;

public class CodebookRecord<TCodebookRecordType> : IComparable
    where TCodebookRecordType : CodebookRecordType
{
    public CodebookRecord()
    {
    }

    [SetsRequiredMembers]
    public CodebookRecord(string stringValue)
    {
        StringValue = stringValue;
    }

    public required string StringValue { get; set; }

    public bool IsValid { get; set; }

    public string? Translation { get; set; }

    public string? Description { get; set; }

    public TCodebookRecordType? AdditionalProperties { get; set; }

    public static implicit operator CodebookRecord<TCodebookRecordType>?(string? value)
    {
        return value == null ? null : new CodebookRecord<TCodebookRecordType> { StringValue = value };
    }

    public static implicit operator string?(CodebookRecord<TCodebookRecordType>? codebookValue) => codebookValue?.StringValue;

    public static bool operator ==(CodebookRecord<TCodebookRecordType>? left, string? right)
        => throw new InvalidOperationException("Cannot compare CodebookRecord<T> with a string directly.");

    public static bool operator !=(CodebookRecord<TCodebookRecordType>? left, string? right)
        => throw new InvalidOperationException("Cannot compare CodebookRecord<T> with a string directly.");

    public static bool operator ==(string? left, CodebookRecord<TCodebookRecordType>? right)
        => throw new InvalidOperationException("Cannot compare string with a CodebookRecord<T> directly.");

    public static bool operator !=(string? left, CodebookRecord<TCodebookRecordType>? right)
        => throw new InvalidOperationException("Cannot compare string with a CodebookRecord<T> directly.");

    public static bool operator ==(CodebookRecord<TCodebookRecordType>? left, CodebookRecord<TCodebookRecordType>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }
        else if (left is null || right is null)
        {
            return false;
        }

        return string.Equals(left.StringValue, right.StringValue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator !=(CodebookRecord<TCodebookRecordType>? left, CodebookRecord<TCodebookRecordType>? right)
    {
        if (left is null && right is null)
        {
            return false;
        }
        else if (left is null || right is null)
        {
            return true;
        }

        return !string.Equals(left.StringValue, right.StringValue, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not CodebookRecord<TCodebookRecordType> obj2)
        {
            return 1;
        }

        return string.Compare(StringValue, obj2.StringValue);
    }

    public override string ToString() => StringValue;

    public override bool Equals(object? obj)
        => obj is CodebookRecord<TCodebookRecordType> other && string.Equals(StringValue, other.StringValue, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => StringValue?.ToLowerInvariant().GetHashCode() ?? 0;
}