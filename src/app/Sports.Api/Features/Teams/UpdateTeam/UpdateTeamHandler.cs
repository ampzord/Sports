namespace Sports.Api.Features.Teams.UpdateTeam;

using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Teams._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

public class UpdateTeamHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<UpdateTeamCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> Handle(
        UpdateTeamCommand command,
        CancellationToken cancellationToken)
    {
        var teamId = TeamId.Create(command.Id);
        var team = await db.Teams.FindAsync([teamId], cancellationToken);

        if (team is null)
            return TeamErrors.NotFound;

        var errors = new List<Error>();

        var nameExists = await db.Teams.AnyAsync(
            t => t.Name == command.Name && t.Id != teamId,
            cancellationToken);

        if (nameExists)
            errors.Add(TeamErrors.NameConflict);

        var newLeagueId = command.LeagueId.HasValue
            ? LeagueId.Create(command.LeagueId.Value)
            : team.LeagueId;

        if (newLeagueId != team.LeagueId)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == newLeagueId, cancellationToken);

            if (!leagueExists)
                errors.Add(LeagueErrors.NotFound);

            var hasMatches = await db.Matches.AnyAsync(
                m => m.HomeTeamId == teamId || m.AwayTeamId == teamId,
                cancellationToken);

            if (hasMatches)
                errors.Add(TeamErrors.HasMatches);
        }

        if (errors.Count > 0)
            return errors;

        team.Update(command.Name, newLeagueId);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(team);
    }
}
