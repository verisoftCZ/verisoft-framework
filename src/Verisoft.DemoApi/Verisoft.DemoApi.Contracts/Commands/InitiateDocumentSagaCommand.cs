namespace Verisoft.DemoApi.Contracts.Commands;

public record InitiateDocumentSagaCommand(Guid DocumentId, string DocumentName, int UserId, DateTime RequestedAt);