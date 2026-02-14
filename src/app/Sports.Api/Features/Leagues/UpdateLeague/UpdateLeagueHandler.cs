namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdateLeagueHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<UpdateLeagueCommand, ErrorOr<LeagueResponse>>
{

    public async Task<ErrorOr<LeagueResponse>> Handle(
        UpdateLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync([command.Id], cancellationToken);

        if (league is null)
            return LeagueErrors.NotFound;

        var nameExists = await db.Leagues.AnyAsync(
            l => l.Name == command.Name && l.Id != command.Id, cancellationToken);

        if (nameExists)
            return LeagueErrors.NameConflict;

        mapper.Apply(command, league);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(league);
    }
}
