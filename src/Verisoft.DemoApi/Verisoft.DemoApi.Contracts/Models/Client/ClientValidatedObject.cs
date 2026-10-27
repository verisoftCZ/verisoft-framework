using Verisoft.Core.Contracts.Validation;

namespace Verisoft.DemoApi.Contracts.Models.Client;

public class ClientValidatedObject : ClientEditModel, IValidatedObject
{
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public List<ValidationProblem>? ValidationProblems { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
}