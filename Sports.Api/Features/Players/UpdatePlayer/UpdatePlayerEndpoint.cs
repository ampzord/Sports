namespace Sports.Api.Features.Players.UpdatePlayer;

using FastEndpoints;
using MediatR;

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
        req.Id = Route<int>("id");
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}