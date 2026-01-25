namespace Sports.Api.Features.Players.DeletePlayer;

using FastEndpoints;
using MediatR;

public class DeletePlayerEndpoint : Endpoint<DeletePlayerRequest>
{
    private readonly IMediator _mediator;
    private readonly DeletePlayerMapper _mapper;

    public DeletePlayerEndpoint(IMediator mediator, DeletePlayerMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Delete("/api/players/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        DeletePlayerRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var deleted = await _mediator.Send(command, ct);

        if (!deleted)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}