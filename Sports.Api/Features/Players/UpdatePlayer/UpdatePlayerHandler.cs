namespace Sports.Api.Features.Players.UpdatePlayer;

using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Players._Shared.Responses;

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
        var player = await db.Players.FirstOrDefaultAsync(
            p => p.Id == command.Id,
            cancellationToken
        );

        if (player is null)
            return Error.NotFound("Player.NotFound", "Player not found");

        var nameExists = await db.Players.AnyAsync(
            p => p.Name == command.Name && p.Id != command.Id,
            cancellationToken);

        if (nameExists)
            return Error.Conflict("Player.NameConflict", $"A player with the name '{command.Name}' already exists");

        mapper.Apply(command, player);

        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(player);
    }
}