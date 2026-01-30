
using FastEndpoints;
using MediatR;

namespace Sports.Api.Features.Matches.UpdateMatch;

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
        Description(b => b
            .Produces<UpdateMatchResponse>(200)
            .Produces(400)
            .Produces(404));
    }

    public override async Task HandleAsync(
        UpdateMatchRequest req,
        CancellationToken ct)
    {
        UpdateMatchCommand command = _mapper.ToCommand(req);
        UpdateMatchResponse? response = await _mediator.Send(command, ct);

        if (response is null)
        {
            _ = await Send.NotFoundAsync(ct);
            return;
        }

        _ = await Send.OkAsync(response, ct);
    }
}