namespace Sports.Api.Features.Teams.UpdateTeam;

using FastEndpoints;
using MediatR;

public class UpdateTeamEndpoint : Endpoint<UpdateTeamRequest, UpdateTeamResponse>
{
    private readonly IMediator _mediator;
    private readonly UpdateTeamMapper _mapper;

    public UpdateTeamEndpoint(IMediator mediator, UpdateTeamMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/teams/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        UpdateTeamRequest req,
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