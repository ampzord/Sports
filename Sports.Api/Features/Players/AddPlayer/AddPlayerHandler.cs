namespace Sports.Api.Features.Players.AddPlayer;

using MediatR;
using Sports.Api.Database;
using Sports.Api.Entities;

public class AddPlayerHandler : IRequestHandler<AddPlayerCommand, AddPlayerResponse>
{
    private readonly SportsDbContext _db;

    public AddPlayerHandler(SportsDbContext db) => _db = db;

    public async Task<AddPlayerResponse> Handle(
        AddPlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = new Player
        {
            Name = command.Name,
            Position = command.Position,
            TeamId = command.TeamId
        };

        _db.Players.Add(player);
        await _db.SaveChangesAsync(cancellationToken);

        return new AddPlayerResponse
        {
            Id = player.Id,
            Name = player.Name,
            Position = player.Position,
            TeamId = player.TeamId
        };
    }
}
