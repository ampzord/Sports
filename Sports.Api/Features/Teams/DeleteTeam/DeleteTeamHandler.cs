namespace Sports.Api.Features.Teams.DeleteTeam;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class DeleteTeamHandler(SportsDbContext db)
    : IRequestHandler<DeleteTeamCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = await db.Teams.FindAsync(command.Id, cancellationToken);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found");

        var hasPlayers = await db.Players.AnyAsync(
            p => p.TeamId == command.Id, cancellationToken);

        if (hasPlayers)
            return Error.Conflict("Team.HasPlayers", "Cannot delete a team that has players");

        var hasMatches = await db.Matches.AnyAsync(
            m => m.HomeTeamId == command.Id || m.AwayTeamId == command.Id, cancellationToken);

        if (hasMatches)
            return Error.Conflict("Team.HasMatches", "Cannot delete a team that has matches");

        db.Teams.Remove(team);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}