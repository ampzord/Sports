namespace Sports.Api.Features.Players.DeletePlayer;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, bool>
{
    private readonly SportsDbContext _db;
    private readonly ILogger<DeletePlayerHandler> _logger;

    public DeletePlayerHandler(
        SportsDbContext db,
        ILogger<DeletePlayerHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<bool> Handle(
        DeletePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await _db.Players.FindAsync(command.Id, cancellationToken);

        if (player is null)
        {
            _logger.LogWarning("Delete failed: Player Id:{PlayerId} not found", command.Id);
            return false;
        }

        _db.Players.Remove(player);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Player deleted with Id:{PlayerId} and Name:{PlayerName}",
            player.Id,
            player.Name);

        return true;
    }
}