namespace Sports.Api.Features.Players.UpdatePlayer;

using Sports.Api.Features.Players._Shared;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdatePlayerHandler(SportsDbContext db, PlayerMapper mapper)
    : IRequestHandler<UpdatePlayerCommand, ErrorOr<PlayerResponse>>
{

    public async Task<ErrorOr<PlayerResponse>> Handle(
        UpdatePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await db.Players.FindAsync([command.Id], cancellationToken);

        if (player is null)
            return PlayerErrors.NotFound;

        var nameExists = await db.Players.AnyAsync(
            p => p.Name == command.Name && p.Id != command.Id,
            cancellationToken);

        if (nameExists)
            return PlayerErrors.NameConflict;

        mapper.Apply(command, player);

        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(player);
    }
}