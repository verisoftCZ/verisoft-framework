using System.ComponentModel.DataAnnotations;

namespace Verisoft.Core.Contracts;

public class FilteredRequest<TFilter>
{
    private int? limit;

    [Range(0, int.MaxValue)]
    public int Offset { get; set; }

    public int Limit
    {
        get => limit is > 0 ? limit.Value : int.MaxValue;
        set => limit = value == int.MaxValue ? null : value;
    }

    public TFilter Filter { get; set; }

    public SortDefinition Sort { get; set; }
}