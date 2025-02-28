using Verisoft.Core.Contracts.Validation;

namespace Verisoft.DemoApi.Contracts.Models.Client;

public class ClientValidatedObject : ClientEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; }
}