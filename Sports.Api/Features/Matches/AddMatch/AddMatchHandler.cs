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
        var errors = new List<Error>();

        var league = await db.Leagues.FindAsync([command.LeagueId], cancellationToken);
        if (league is null)
            errors.Add(Error.NotFound("League.NotFound", "League not found"));

        var homeTeam = await db.Teams.FindAsync([command.HomeTeamId], cancellationToken);
        if (homeTeam is null)
            errors.Add(Error.NotFound("HomeTeam.NotFound", "Home team not found"));

        var awayTeam = await db.Teams.FindAsync([command.AwayTeamId], cancellationToken);
        if (awayTeam is null)
            errors.Add(Error.NotFound("AwayTeam.NotFound", "Away team not found"));

        if (errors.Count > 0)
            return errors;

        if (homeTeam!.LeagueId != awayTeam!.LeagueId)
            errors.Add(Error.Validation("Match.DifferentLeagues", "Both teams must belong to the same league"));

        if (homeTeam.LeagueId != command.LeagueId)
            errors.Add(Error.Validation("Match.LeagueMismatch", "Teams do not belong to the specified league"));

        if (errors.Count > 0)
            return errors;

        var match = mapper.ToEntity(command);

        db.Matches.Add(match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}