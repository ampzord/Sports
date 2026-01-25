namespace Sports.Api.Features.Teams.DeleteTeam;

using FastEndpoints;
using MediatR;

public class DeleteTeamEndpoint : Endpoint<DeleteTeamRequest, DeleteTeamResponse>
{
    private readonly IMediator _mediator;
    private readonly DeleteTeamMapper _mapper;

    public DeleteTeamEndpoint(IMediator mediator, DeleteTeamMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Delete("/api/teams/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        DeleteTeamRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}