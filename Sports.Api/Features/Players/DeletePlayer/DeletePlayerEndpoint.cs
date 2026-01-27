namespace Sports.Api.Features.Players.DeletePlayer;

using FastEndpoints;
using MediatR;

public class DeletePlayerEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public DeletePlayerEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Delete("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .Produces(404));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var command = new DeletePlayerCommand(id);
        var deleted = await _mediator.Send(command, ct);

        if (!deleted)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}