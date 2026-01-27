namespace Sports.Api.Features.Players.GetPlayer;

using FastEndpoints;
using MediatR;

public class GetPlayerEndpoint : EndpointWithoutRequest<GetPlayerResponse>
{
    private readonly IMediator _mediator;
    private readonly GetPlayerMapper _mapper;

    public GetPlayerEndpoint(IMediator mediator, GetPlayerMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/players/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<GetPlayerResponse>(200)
            .Produces(404));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var query = new GetPlayerQuery(id);
        var response = await _mediator.Send(query, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}