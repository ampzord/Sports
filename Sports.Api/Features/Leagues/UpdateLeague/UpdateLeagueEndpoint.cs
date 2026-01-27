namespace Sports.Api.Features.Leagues.UpdateLeague;

using FastEndpoints;
using MediatR;

public class UpdateLeagueEndpoint : Endpoint<UpdateLeagueRequest, UpdateLeagueResponse>
{
    private readonly IMediator _mediator;
    private readonly UpdateLeagueMapper _mapper;

    public UpdateLeagueEndpoint(IMediator mediator, UpdateLeagueMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/leagues/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<UpdateLeagueResponse>(200)
            .Produces(400)
            .Produces(404));
    }

    public override async Task HandleAsync(
        UpdateLeagueRequest req,
        CancellationToken ct)
    {
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