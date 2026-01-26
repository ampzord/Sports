namespace Sports.Api.Features.Matches.UpdateMatch;

using FastEndpoints;
using MediatR;

public class UpdateMatchEndpoint : Endpoint<UpdateMatchRequest, UpdateMatchResponse>
{
    private readonly IMediator _mediator;
    private readonly UpdateMatchMapper _mapper;

    public UpdateMatchEndpoint(IMediator mediator, UpdateMatchMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/matches/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        UpdateMatchRequest req,
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