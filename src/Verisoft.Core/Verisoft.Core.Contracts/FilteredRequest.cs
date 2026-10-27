using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Verisoft.Core.Contracts;

public class FilteredRequest<TFilter>
{
    private int? limit;

    private List<string>? sorts;

    [Range(0, int.MaxValue)]
    public int Offset { get; set; }

    public int Limit
    {
        get => limit is > 0 ? limit.Value : int.MaxValue;
        set => limit = value == int.MaxValue ? null : value;
    }

    public TFilter? Filter { get; set; }

    public SortDefinition? Sort { get; set; }

    public List<string>? Sorts
    {
        get
        {
            if (sorts == null && Sort != null)
            {
                sorts = new List<string> { $"{Sort.Field},{Sort.Direction}" };
            }

            return sorts;
        }

        set
        {
            sorts = value;
        }
    }

    [BindNever]
    [JsonIgnore]
    public IEnumerable<SortDefinition> ParsedSorts =>
        (Sorts ?? Enumerable.Empty<string>()).Select(item =>
        {
            var parts = item.Split(',', 2, StringSplitOptions.TrimEntries);
            return new SortDefinition
            {
                Field = parts[0],
                Direction = parts.Length > 1 &&
                            Enum.TryParse<SortDirection>(parts[1], true, out var dir)
                                ? dir
                                : SortDirection.Asc,
            };
        });
}