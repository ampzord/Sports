namespace Sports.Api.Features.Players.UpdatePlayer;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Api.Features.Players._Shared;
using Sports.Domain.PlayerAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

public class UpdatePlayerHandler(SportsDbContext db, PlayerMapper mapper)
    : IRequestHandler<UpdatePlayerCommand, ErrorOr<PlayerResponse>>
{

    public async Task<ErrorOr<PlayerResponse>> Handle(
        UpdatePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var playerId = PlayerId.Create(command.Id);
        var player = await db.Players.FindAsync([playerId], cancellationToken);

        if (player is null)
            return PlayerErrors.NotFound;

        var nameExists = await db.Players.AnyAsync(
            p => p.Name == command.Name && p.Id != playerId,
            cancellationToken);

        if (nameExists)
            return PlayerErrors.NameConflict;

        player.Update(command.Name, command.Position, TeamId.Create(command.TeamId));

        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(player);
    }
}
