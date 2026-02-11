namespace Sports.Api.Features.Leagues.DeleteLeague;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;

public class DeleteLeagueHandler(SportsDbContext db)
    : IRequestHandler<DeleteLeagueCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync([command.Id], cancellationToken);

        if (league is null)
            return LeagueErrors.NotFound;

        var hasTeams = await db.Teams.AnyAsync(
            t => t.LeagueId == command.Id, cancellationToken);

        if (hasTeams)
            return LeagueErrors.HasTeams;

        db.Leagues.Remove(league);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}