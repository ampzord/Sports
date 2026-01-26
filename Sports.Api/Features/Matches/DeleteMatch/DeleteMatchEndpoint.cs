namespace Sports.Api.Features.Matches.DeleteMatch;

using FastEndpoints;
using MediatR;

public class DeleteMatchEndpoint : Endpoint<DeleteMatchRequest, DeleteMatchResponse>
{
    private readonly IMediator _mediator;
    private readonly DeleteMatchMapper _mapper;

    public DeleteMatchEndpoint(IMediator mediator, DeleteMatchMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Delete("/api/matches/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        DeleteMatchRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}