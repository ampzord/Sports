namespace Sports.Api.Features.Teams.GetTeam;

using FastEndpoints;
using MediatR;

public class GetTeamEndpoint : Endpoint<GetTeamRequest, GetTeamResponse>
{
    private readonly IMediator _mediator;
    private readonly GetTeamMapper _mapper;

    public GetTeamEndpoint(IMediator mediator, GetTeamMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/teams/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetTeamRequest req,
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