namespace Verisoft.Core.Contracts
{
    public class SortDefinition
    {
        public string Field { get; set; }

        public SortDirection Direction { get; set; } = SortDirection.Asc;
    }
}
