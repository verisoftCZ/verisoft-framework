namespace Verisoft.DemoApi.Contracts.Models.Client;

public class Client : ClientEditModel
{
    public int Id { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
