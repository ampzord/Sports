namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class UpdateLeagueHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<UpdateLeagueCommand, ErrorOr<LeagueResponse>>
{

    public async Task<ErrorOr<LeagueResponse>> Handle(
        UpdateLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var leagueId = LeagueId.Create(command.Id);
        var league = await db.Leagues.FindAsync([leagueId], cancellationToken);

        if (league is null)
            return LeagueErrors.NotFound;

        var nameExists = await db.Leagues.AnyAsync(
            l => l.Name == command.Name && l.Id != leagueId, cancellationToken);

        if (nameExists)
            return LeagueErrors.NameConflict;

        league.UpdateName(command.Name);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(league);
    }
}
