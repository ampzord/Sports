namespace Sports.Api.Features.Leagues.DeleteLeague;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class DeleteLeagueHandler(SportsDbContext db)
    : IRequestHandler<DeleteLeagueCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync([command.Id], cancellationToken);

        if (league is null)
            return Error.NotFound("League.NotFound", "League not found");

        var hasTeams = await db.Teams.AnyAsync(
            t => t.LeagueId == command.Id, cancellationToken);

        if (hasTeams)
            return Error.Conflict("League.HasTeams", "Cannot delete a league that has teams");

        db.Leagues.Remove(league);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}