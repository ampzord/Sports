namespace Sports.Api.Features.Matches.GetMatches;

using Sports.Api.Features.Matches._Shared.Responses;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetMatchesEndpoint(IMediator mediator) :
    Endpoint<GetMatchesRequest, List<MatchResponse>>
{

    public override void Configure()
    {
        Get("/api/matches");
        AllowAnonymous();
        Description(b => b
            .Produces<List<MatchResponse>>(200)
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
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
