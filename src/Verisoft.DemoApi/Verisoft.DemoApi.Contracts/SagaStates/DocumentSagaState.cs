using MassTransit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Verisoft.DemoApi.Contracts.Sagas;

[Table("DocumentSagaState", Schema = "MassTransit")]
public class DocumentSagaState : SagaStateMachineInstance
{
    [Key]
    public Guid CorrelationId { get; set; }

    public string? CurrentState { get; set; }

    public string? DocumentName { get; set; }

    public DateTime Created { get; set; }

    public DateTime RequestedAt { get; set; }
}
