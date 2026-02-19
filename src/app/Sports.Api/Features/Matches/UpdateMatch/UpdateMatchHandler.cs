namespace Sports.Api.Features.Matches.UpdateMatch;

using Sports.Api.Features.Matches._Shared;
using ErrorOr;
using MediatR;
using Sports.Api.Database;
using Sports.Domain.MatchAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

public class UpdateMatchHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<UpdateMatchCommand, ErrorOr<MatchResponse>>
{
    public async Task<ErrorOr<MatchResponse>> Handle(
        UpdateMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync([MatchId.Create(command.Id)], cancellationToken);

        if (match is null)
            return MatchErrors.NotFound;

        var errors = new List<Error>();

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

        if (errors.Count > 0)
            return errors;

        match.Update(TeamId.Create(command.HomeTeamId), TeamId.Create(command.AwayTeamId), command.TotalPasses);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}
