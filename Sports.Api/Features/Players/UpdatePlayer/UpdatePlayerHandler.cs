namespace Sports.Api.Features.Players.UpdatePlayer;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdatePlayerHandler
    : IRequestHandler<UpdatePlayerCommand, UpdatePlayerResponse?>
{
    private readonly SportsDbContext _db;
    private readonly UpdatePlayerMapper _mapper;

    public UpdatePlayerHandler(SportsDbContext db, UpdatePlayerMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UpdatePlayerResponse?> Handle(
        UpdatePlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = await _db.Players.FirstOrDefaultAsync(
            p => p.Id == command.Id,
            cancellationToken
        );

        if (player is null)
            return null;

        _mapper.Apply(command, player);

        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(player);
    }
}