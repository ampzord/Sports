namespace Sports.Api.Features.Matches.AddMatch;

using ErrorOr;
using MediatR;
using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Matches._Shared;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.MatchAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public class AddMatchHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<AddMatchCommand, ErrorOr<MatchResponse>>
{

    public async Task<ErrorOr<MatchResponse>> Handle(
        AddMatchCommand command,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var league = await db.Leagues.FindAsync([LeagueId.Create(command.LeagueId)], cancellationToken);
        if (league is null)
            errors.Add(LeagueErrors.NotFound);

        var homeTeam = await db.Teams.FindAsync([TeamId.Create(command.HomeTeamId)], cancellationToken);
        if (homeTeam is null)
            errors.Add(MatchErrors.HomeTeamNotFound);

        var awayTeam = await db.Teams.FindAsync([TeamId.Create(command.AwayTeamId)], cancellationToken);
        if (awayTeam is null)
            errors.Add(MatchErrors.AwayTeamNotFound);

        if (errors.Count > 0)
            return errors;

        if (homeTeam!.LeagueId != awayTeam!.LeagueId)
            errors.Add(MatchErrors.DifferentLeagues);

        if (homeTeam.LeagueId != LeagueId.Create(command.LeagueId))
            errors.Add(MatchErrors.LeagueMismatch);

        if (errors.Count > 0)
            return errors;

        var match = Match.Create(TeamId.Create(command.HomeTeamId), TeamId.Create(command.AwayTeamId), command.TotalPasses);

        db.Matches.Add(match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}