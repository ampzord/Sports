namespace Sports.Api.Features.Players.GetPlayer;

using FastEndpoints;
using MediatR;

public class GetPlayerEndpoint : Endpoint<GetPlayerRequest, GetPlayerResponse>
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
    }

    public override async Task HandleAsync(
        GetPlayerRequest req,
        CancellationToken ct)
    {
        var query = _mapper.ToQuery(req);
        var response = await _mediator.Send(query, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}