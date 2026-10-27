namespace Verisoft.Core.Codebooks.Common.Models;

public class Codebook : CodebookEditModel
{
    public int Id { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
