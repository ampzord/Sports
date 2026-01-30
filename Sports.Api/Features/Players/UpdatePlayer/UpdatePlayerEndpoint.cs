
using FastEndpoints;
using MediatR;

namespace Sports.Api.Features.Players.UpdatePlayer;

public class UpdatePlayerEndpoint : Endpoint<UpdatePlayerRequest, UpdatePlayerResponse>
{
    private readonly IMediator _mediator;
    private readonly UpdatePlayerMapper _mapper;

    public UpdatePlayerEndpoint(IMediator mediator, UpdatePlayerMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<UpdatePlayerResponse>(200)
            .Produces(400)
            .Produces(404));
    }

    public override async Task HandleAsync(
        UpdatePlayerRequest req,
        CancellationToken ct)
    {
        UpdatePlayerCommand command = _mapper.ToCommand(req);
        UpdatePlayerResponse? response = await _mediator.Send(command, ct);

        if (response is null)
        {
            _ = await Send.NotFoundAsync(ct);
            return;
        }

        _ = await Send.OkAsync(response, ct);
    }
}