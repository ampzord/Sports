
using FastEndpoints;
using MediatR;

namespace Sports.Api.Features.Teams.UpdateTeam;

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
        UpdateTeamCommand command = _mapper.ToCommand(req);
        UpdateTeamResponse? response = await _mediator.Send(command, ct);

        if (response is null)
        {
            _ = await Send.NotFoundAsync(ct);
            return;
        }

        _ = await Send.OkAsync(response, ct);
    }
}