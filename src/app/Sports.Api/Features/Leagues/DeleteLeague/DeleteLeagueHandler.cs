namespace Sports.Api.Features.Leagues.DeleteLeague;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Features.Leagues._Shared;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class DeleteLeagueHandler(SportsDbContext db)
    : IRequestHandler<DeleteLeagueCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var leagueId = LeagueId.Create(command.Id);
        var league = await db.Leagues.FindAsync([leagueId], cancellationToken);

        if (league is null)
            return LeagueErrors.NotFound;

        var hasTeams = await db.Teams.AnyAsync(
            t => t.LeagueId == leagueId, cancellationToken);

        if (hasTeams)
            return LeagueErrors.HasTeams;

        db.Leagues.Remove(league);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}