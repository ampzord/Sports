namespace Sports.Api.Features.Matches.GetMatch;

using FastEndpoints;
using MediatR;

public class GetMatchEndpoint : Endpoint<GetMatchRequest, GetMatchResponse>
{
    private readonly IMediator _mediator;
    private readonly GetMatchMapper _mapper;

    public GetMatchEndpoint(IMediator mediator, GetMatchMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/matches/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetMatchRequest req,
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