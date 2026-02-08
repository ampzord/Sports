namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class AddPlayerHandler(SportsDbContext db, PlayerMapper mapper)
    : IRequestHandler<AddPlayerCommand, ErrorOr<PlayerResponse>>
{

    public async Task<ErrorOr<PlayerResponse>> Handle(
        AddPlayerCommand command,
        CancellationToken cancellationToken)
    {
        var teamExists = await db.Teams.AnyAsync(
            t => t.Id == command.TeamId,
            cancellationToken);

        if (!teamExists)
            return Error.NotFound("Team.NotFound", "Team not found");

        var nameExists = await db.Players.AnyAsync(
            p => p.Name == command.Name,
            cancellationToken);

        if (nameExists)
            return Error.Conflict("Player.NameConflict", $"A player with the name '{command.Name}' already exists");

        var player = mapper.ToEntity(command);

        db.Players.Add(player);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(player);
    }
}