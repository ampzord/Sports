namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Teams._Shared;

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
        var errors = new List<Error>();

        var teamExists = await db.Teams.AnyAsync(
            t => t.Id == command.TeamId,
            cancellationToken);

        if (!teamExists)
            errors.Add(TeamErrors.NotFound);

        var nameExists = await db.Players.AnyAsync(
            p => p.Name == command.Name,
            cancellationToken);

        if (nameExists)
            errors.Add(PlayerErrors.NameConflict);

        if (errors.Count > 0)
            return errors;

        var player = mapper.ToEntity(command);

        db.Players.Add(player);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(player);
    }
}