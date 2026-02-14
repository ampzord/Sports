namespace Sports.Api.Features.Players.GetPlayers;

using System.Collections.Immutable;
using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Teams._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetPlayersHandler(SportsDbContext db, PlayerMapper mapper, ILogger<GetPlayersHandler> logger)
    : IRequestHandler<GetPlayersQuery, ErrorOr<ImmutableList<PlayerResponse>>>
{
    public async Task<ErrorOr<ImmutableList<PlayerResponse>>> Handle(
        GetPlayersQuery query,
        CancellationToken cancellationToken)
    {
        if (query.TeamId is not null)
        {
            var teamExists = await db.Teams.AnyAsync(
                t => t.Id == query.TeamId, cancellationToken);

            if (!teamExists)
                return TeamErrors.NotFound;
        }

        var players = await db.Players
            .AsNoTracking()
            .WhereIf(query.TeamId.HasValue,
                p => p.TeamId == query.TeamId)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Retrieved {Count} players", players.Count);

        return mapper.ToResponseList(players);
    }
}
