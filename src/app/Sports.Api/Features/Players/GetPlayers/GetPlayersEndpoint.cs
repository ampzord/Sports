namespace Sports.Api.Features.Players.GetPlayers;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Players._Shared;
using System.Collections.Immutable;

public class GetPlayersEndpoint(IMediator mediator) :
    Endpoint<GetPlayersRequest, ImmutableList<PlayerResponse>>
{

    public override void Configure()
    {
        Get("players");
        AllowAnonymous();
        Description(b => b
            .Produces<ImmutableList<PlayerResponse>>(200)
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
