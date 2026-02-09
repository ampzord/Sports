using Sports.Api.Features.Players._Shared;

namespace Sports.Api.Features.Players.GetPlayers;


using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetPlayersEndpoint(IMediator mediator) :
    Endpoint<GetPlayersRequest, List<PlayerResponse>>
{

    public override void Configure()
    {
        Get("/api/players");
        AllowAnonymous();
        Description(b => b
            .Produces<List<PlayerResponse>>(200)
            .WithTags("Players"));
        Summary(s =>
        {
            s.Summary = "Get all players";
        });
    }

    public override async Task HandleAsync(
        GetPlayersRequest req,
        CancellationToken ct)
    {
        var query = new GetPlayersQuery(req.TeamId);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
