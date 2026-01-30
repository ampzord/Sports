
using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

namespace Sports.Api.Features.Leagues.UpdateLeague;

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
        UpdateLeagueCommand command = _mapper.ToCommand(req);
        UpdateLeagueResponse? response = await _mediator.Send(command, ct);

        if (response is null)
        {
            //ThrowError("League not found", "NOT_FOUND", statusCode: StatusCodes.Status404NotFound);
            this.ThrowNotFound("League not found");
            return;
        }

        await Send.OkAsync(response, ct);
    }
}