using System.Collections.Immutable;
using Sports.Api.Features.Matches._Shared;

namespace Sports.Api.Features.Matches.GetMatches;


using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetMatchesEndpoint(IMediator mediator) :
    Endpoint<GetMatchesRequest, ImmutableList<MatchResponse>>
{

    public override void Configure()
    {
        Get("/api/matches");
        AllowAnonymous();
        Description(b => b
            .Produces<ImmutableList<MatchResponse>>(200)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Get all matches";
        });
    }

    public override async Task HandleAsync(
        GetMatchesRequest req,
        CancellationToken ct)
    {
        var query = new GetMatchesQuery(req.LeagueId);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
