namespace Sports.Api.Features.Players.DeletePlayer;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class DeletePlayerHandler(SportsDbContext db)
    : IRequestHandler<DeletePlayerCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeletePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await db.Players.FindAsync(command.Id, cancellationToken);

        if (player is null)
            return Error.NotFound("Player.NotFound", "Player not found");

        db.Players.Remove(player);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}