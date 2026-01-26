namespace Sports.Api.Features.Leagues.AddLeague;

using FastEndpoints;
using MediatR;

public class AddLeagueEndpoint : Endpoint<AddLeagueRequest, AddLeagueResponse>
{
    private readonly IMediator _mediator;
    private readonly AddLeagueMapper _mapper;

    public AddLeagueEndpoint(IMediator mediator, AddLeagueMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/api/leagues");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddLeagueRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}