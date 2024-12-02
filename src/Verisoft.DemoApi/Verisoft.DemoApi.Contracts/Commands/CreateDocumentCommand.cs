namespace Verisoft.DemoApi.Contracts.Commands;

public record CreateDocumentCommand(Guid DocumentId, string DocumentName, int ClientOwnerId);