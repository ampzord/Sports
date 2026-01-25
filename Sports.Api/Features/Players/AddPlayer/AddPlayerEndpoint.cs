namespace Sports.Api.Features.Players.AddPlayer;

using FastEndpoints;
using MediatR;

public class AddPlayerEndpoint : Endpoint<AddPlayerRequest, AddPlayerResponse>
{
    private readonly IMediator _mediator;

    public AddPlayerEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Post("/api/players");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddPlayerRequest req,
        CancellationToken ct)
    {
        var command = new AddPlayerCommand(req.Name, req.Position, req.TeamId);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}