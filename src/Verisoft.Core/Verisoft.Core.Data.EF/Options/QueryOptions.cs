namespace Verisoft.Core.Data.EntityFramework.Options
{
    public class QueryOptions
    {
        public static QueryOptions Default => new()
        {
                AsNoTracking = true,
                AsSplitQuery = false,
        };

        public bool AsNoTracking { get; set; } = true;

        public bool AsSplitQuery { get; set; } = false;
    }
}
