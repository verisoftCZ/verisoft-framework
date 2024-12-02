namespace Verisoft.DemoApi.Contracts.Models.Template;

public class MainData
{
    public string Header { get; set; }

    public DateTime Date { get; set; }

    public List<MainDataItem> ListData { get; set; }
}