namespace Verisoft.Core.Codebooks.Configurations;

public class CustomEndpointDefinition
{
    public required string Route { get; set; }

    public required HttpMethod Method { get; set; }

    public required Delegate Handler { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<(Type ResponseType, int StatusCode)> ResponseTypes { get; } = [];
}