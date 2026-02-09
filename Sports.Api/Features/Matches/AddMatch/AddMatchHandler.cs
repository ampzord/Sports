namespace Sports.Api.Features.Matches.AddMatch;

using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class AddMatchHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<AddMatchCommand, ErrorOr<MatchResponse>>
{

    public async Task<ErrorOr<MatchResponse>> Handle(
        AddMatchCommand command,
        CancellationToken cancellationToken)
    {
        var homeTeam = await db.Teams.FindAsync([command.HomeTeamId], cancellationToken);

        if (homeTeam is null)
            return Error.NotFound("HomeTeam.NotFound", "Home team not found");

        var awayTeam = await db.Teams.FindAsync([command.AwayTeamId], cancellationToken);

        if (awayTeam is null)
            return Error.NotFound("AwayTeam.NotFound", "Away team not found");

        if (homeTeam.LeagueId != awayTeam.LeagueId)
            return Error.Validation("Match.DifferentLeagues", "Both teams must belong to the same league");

        if (homeTeam.LeagueId != command.LeagueId)
            return Error.Validation("Match.LeagueMismatch", "Teams do not belong to the specified league");

        var match = mapper.ToEntity(command);

        db.Matches.Add(match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}