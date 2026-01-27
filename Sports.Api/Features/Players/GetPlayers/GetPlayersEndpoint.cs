namespace Sports.Api.Features.Players.GetPlayers;

using FastEndpoints;
using MediatR;

public class GetPlayersEndpoint : EndpointWithoutRequest<List<GetPlayersResponse>>
{
    private readonly IMediator _mediator;

    public GetPlayersEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Get("/api/players");
        AllowAnonymous();
        Description(b => b.Produces<List<GetPlayersResponse>>(200));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetPlayersQuery();
        var response = await _mediator.Send(query, ct);
        await Send.OkAsync(response, ct);
    }
}