
using FastEndpoints;
using MediatR;

namespace Sports.MatchSimulator.Features.MatchSimulation;

public class MatchSimulationEndpoint
    : EndpointWithoutRequest<MatchSimulationResponse>
{
    private readonly IMediator _mediator;

    public MatchSimulationEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/api/matches/simulate");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        MatchSimulationCommand command = new();
        MatchSimulationResponse response = await _mediator.Send(command, ct);
        _ = await Send.OkAsync(response, cancellation: ct);
    }
}