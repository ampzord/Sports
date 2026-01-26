namespace Sports.Api.Features.Matches.AddMatch;

using FastEndpoints;
using MediatR;

public class AddMatchEndpoint : Endpoint<AddMatchRequest, AddMatchResponse>
{
    private readonly IMediator _mediator;
    private readonly AddMatchMapper _mapper;

    public AddMatchEndpoint(IMediator mediator, AddMatchMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/api/matches");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddMatchRequest req,
        CancellationToken ct)
    {
        var command = _mapper.ToCommand(req);
        var response = await _mediator.Send(command, ct);
        await Send.OkAsync(response, ct);
    }
}