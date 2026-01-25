namespace Sports.Api.Features.Teams.AddTeam;

using FastEndpoints;
using MediatR;

public class AddTeamEndpoint : Endpoint<AddTeamRequest, AddTeamResponse>
{
    private readonly IMediator _mediator;
    private readonly AddTeamMapper _mapper;

    public AddTeamEndpoint(IMediator mediator, AddTeamMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/api/teams");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddTeamRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}