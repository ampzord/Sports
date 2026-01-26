namespace Sports.Api.Features.Leagues.DeleteLeague;

using FastEndpoints;
using MediatR;

public class DeleteLeagueEndpoint : Endpoint<DeleteLeagueRequest, DeleteLeagueResponse>
{
    private readonly IMediator _mediator;
    private readonly DeleteLeagueMapper _mapper;

    public DeleteLeagueEndpoint(IMediator mediator, DeleteLeagueMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Delete("/api/leagues/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        DeleteLeagueRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}