namespace Sports.Api.Features.Teams.DeleteTeam;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Features.Teams._Shared;
using Sports.Domain.TeamAggregate.ValueObjects;

public class DeleteTeamHandler(SportsDbContext db)
    : IRequestHandler<DeleteTeamCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTeamCommand command,
        CancellationToken cancellationToken)
    {
        var teamId = TeamId.Create(command.Id);
        var team = await db.Teams.FindAsync([teamId], cancellationToken);

        if (team is null)
            return TeamErrors.NotFound;

        var hasPlayers = await db.Players.AnyAsync(
            p => p.TeamId == teamId, cancellationToken);

        if (hasPlayers)
            return TeamErrors.HasPlayers;

        var hasMatches = await db.Matches.AnyAsync(
            m => m.HomeTeamId == teamId || m.AwayTeamId == teamId, cancellationToken);

        if (hasMatches)
            return TeamErrors.HasMatches;

        db.Teams.Remove(team);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}