using System;
using System.Collections.Generic;

namespace Verisoft.Core.Common.Entities.Codebooks;

public class CodebookRecordEntity<T> : BaseEntity<int>
        where T : CodebookRecordType, new()
{
    public string StringValue { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsValid => ValidFrom <= DateOnly.FromDateTime(DateTime.UtcNow) && (ValidTo == null || ValidTo >= DateOnly.FromDateTime(DateTime.UtcNow));

    public DateOnly ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }

    public T AdditionalProperties { get; set; } = new();

    public HashSet<CodebookRecordTranslationEntity<T>> Translations { get; set; } = [];

    public static implicit operator CodebookRecordEntity<T>(string stringValue)
    {
        return new CodebookRecordEntity<T> { StringValue = stringValue };
    }

    public static implicit operator CodebookRecordEntity<T>(CodebookRecord<T> record)
    {
        return record.StringValue;
    }

    public static bool operator ==(CodebookRecordEntity<T> left, CodebookRecord<T> right)
    {
        return left?.StringValue == right?.StringValue;
    }

    public static bool operator !=(CodebookRecordEntity<T> left, CodebookRecord<T> right)
    {
        return left?.StringValue != right?.StringValue;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CodebookRecordEntity<T> entity)
        {
            return StringValue == entity.StringValue;
        }

        if (obj is CodebookRecord<T> value)
        {
            return StringValue == value.StringValue;
        }

        if (obj is string str)
        {
            return StringValue == str;
        }

        return false;
    }

    public override int GetHashCode() => StringValue.GetHashCode();
}