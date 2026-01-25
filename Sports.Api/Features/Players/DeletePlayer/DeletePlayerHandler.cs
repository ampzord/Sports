namespace Sports.Api.Features.Players.DeletePlayer;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, bool>
{
    private readonly SportsDbContext _db;

    public DeletePlayerHandler(SportsDbContext db) => _db = db;

    public async Task<bool> Handle(
        DeletePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await _db.Players
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (player is null)
            return false;

        _db.Players.Remove(player);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}