namespace Verisoft.DemoApi.Contracts.Filters
{
    public class DocumentFilter
    {
        public string[] Name { get; set; }

        public string[] ContentType { get; set; }

        public string SearchField { get; set; }
    }
}
