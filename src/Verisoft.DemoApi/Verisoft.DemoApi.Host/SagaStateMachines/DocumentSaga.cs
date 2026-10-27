using MassTransit;
using Verisoft.DemoApi.Contracts.Commands;
using Verisoft.DemoApi.Contracts.Sagas;

namespace Verisoft.DemoApi.Host.SagaStateMachines;

/// <summary>
/// This is the state machine that defines the saga's behavior.
/// </summary>
public class DocumentSaga : MassTransitStateMachine<DocumentSagaState>
{
    public DocumentSaga(ILogger<DocumentSaga> logger)
    {
        InstanceState(x => x.CurrentState);

        Event(() => InitiateDocument, x => x.CorrelateById(context => context.Message.DocumentId));
        Event(() => DocumentCompleted, x => x.CorrelateById(context => context.Message.DocumentId));

        Initially(
            When(InitiateDocument)
                .Then(context =>
                {
                    context.Saga.Created = DateTime.UtcNow;
                    context.Saga.RequestedAt = context.Message.RequestedAt;
                    context.Saga.DocumentName = context.Message.DocumentName;

                    logger.LogInformation(
                        "Saga started for Document: {DocumentName} ({CorrelationId}) at {RequestedAt}",
                        context.Saga.DocumentName,
                        context.Saga.CorrelationId,
                        context.Saga.RequestedAt);
                })
                .TransitionTo(Creating));

        During(Creating, When(DocumentCompleted)
                .Then(context =>
                {
                    logger.LogInformation(
                        "Saga completed for Document: {DocumentName} ({CorrelationId})",
                        context.Saga.DocumentName,
                        context.Saga.CorrelationId);
                })
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public State? Creating { get; private set; } = default;

    public Event<InitiateDocumentSagaCommand>? InitiateDocument { get; private set; } = default;

    public Event<CompleteDocumentSagaCommand>? DocumentCompleted { get; private set; } = default;
}