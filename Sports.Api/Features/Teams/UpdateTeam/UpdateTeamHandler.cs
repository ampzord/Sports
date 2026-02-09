namespace Sports.Api.Features.Teams.UpdateTeam;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdateTeamHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<UpdateTeamCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> Handle(
        UpdateTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = await db.Teams.FindAsync([command.Id], cancellationToken);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found");

        var errors = new List<Error>();

        var nameExists = await db.Teams.AnyAsync(
            t => t.Name == command.Name && t.Id != command.Id,
            cancellationToken);

        if (nameExists)
            errors.Add(Error.Conflict("Team.NameConflict", $"A team with the name '{command.Name}' already exists"));

        if (command.LeagueId.HasValue && command.LeagueId != team.LeagueId)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == command.LeagueId, cancellationToken);

            if (!leagueExists)
                errors.Add(Error.NotFound("League.NotFound", "League not found"));

            var hasMatches = await db.Matches.AnyAsync(
                m => m.HomeTeamId == command.Id || m.AwayTeamId == command.Id,
                cancellationToken);

            if (hasMatches)
                errors.Add(Error.Validation("Team.HasMatches", "Cannot change league for a team that has existing matches"));
        }

        if (errors.Count > 0)
            return errors;

        mapper.Apply(command, team);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(team);
    }
}