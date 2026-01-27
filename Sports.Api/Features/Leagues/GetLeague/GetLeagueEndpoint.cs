namespace Sports.Api.Features.Leagues.GetLeague;

using FastEndpoints;
using MediatR;

public class GetLeagueEndpoint : Endpoint<GetLeagueRequest, GetLeagueResponse>
{
    private readonly IMediator _mediator;
    private readonly GetLeagueMapper _mapper;

    public GetLeagueEndpoint(IMediator mediator, GetLeagueMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/leagues/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<GetLeagueResponse>(200)
            .Produces(404));
    }

    public override async Task HandleAsync(
        GetLeagueRequest req,
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