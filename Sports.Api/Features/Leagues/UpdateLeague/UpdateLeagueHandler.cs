namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdateLeagueHandler(SportsDbContext db, UpdateLeagueMapper mapper)
    : IRequestHandler<UpdateLeagueCommand, ErrorOr<LeagueResponse>>
{

    public async Task<ErrorOr<LeagueResponse>> Handle(
        UpdateLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync(command.Id, cancellationToken);

        if (league is null)
            return Error.NotFound("League.NotFound", "League not found");

        var nameExists = await db.Leagues.AnyAsync(
            l => l.Name == command.Name && l.Id != command.Id, cancellationToken);

        if (nameExists)
            return Error.Conflict("League.NameConflict", $"A league with the name '{command.Name}' already exists");

        mapper.Apply(command, league);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(league);
    }
}