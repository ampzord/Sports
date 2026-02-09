namespace Sports.Api.Features.Matches.UpdateMatch;

using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class UpdateMatchHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<UpdateMatchCommand, ErrorOr<MatchResponse>>
{
    public async Task<ErrorOr<MatchResponse>> Handle(
        UpdateMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync([command.Id], cancellationToken);

        if (match is null)
            return Error.NotFound("Match.NotFound", "Match not found");

        var homeTeam = await db.Teams.FindAsync([command.HomeTeamId], cancellationToken);

        if (homeTeam is null)
            return Error.NotFound("HomeTeam.NotFound", "Home team not found");

        var awayTeam = await db.Teams.FindAsync([command.AwayTeamId], cancellationToken);

        if (awayTeam is null)
            return Error.NotFound("AwayTeam.NotFound", "Away team not found");

        if (homeTeam.LeagueId != awayTeam.LeagueId)
            return Error.Validation("Match.DifferentLeagues", "Both teams must belong to the same league");

        mapper.Apply(command, match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}