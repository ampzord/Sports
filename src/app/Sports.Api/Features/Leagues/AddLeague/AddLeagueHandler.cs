namespace Sports.Api.Features.Leagues.AddLeague;

using Sports.Api.Features.Leagues._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Domain.LeagueAggregate;

public class AddLeagueHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<AddLeagueCommand, ErrorOr<LeagueResponse>>
{
    public async Task<ErrorOr<LeagueResponse>> Handle(
        AddLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var nameExists = await db.Leagues.AnyAsync(
            l => l.Name == command.Name, cancellationToken);

        if (nameExists)
            return LeagueErrors.NameConflict;

        var league = League.Create(command.Name);

        db.Leagues.Add(league);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(league);
    }
}
