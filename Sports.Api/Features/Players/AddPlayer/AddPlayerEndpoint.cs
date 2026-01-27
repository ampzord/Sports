namespace Sports.Api.Features.Players.AddPlayer;

using FastEndpoints;
using MediatR;

public class AddPlayerEndpoint : Endpoint<AddPlayerRequest, AddPlayerResponse>
{
    private readonly IMediator _mediator;
    private readonly AddPlayerMapper _mapper;

    public AddPlayerEndpoint(IMediator mediator, AddPlayerMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/api/players");
        AllowAnonymous();
        Description(b => b
            .Produces<AddPlayerResponse>(201)
            .Produces(400));
    }

    public override async Task HandleAsync(
        AddPlayerRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}