namespace Sports.Api.Features.Players.GetPlayers;

using System.Collections.Immutable;
using Sports.Api.Features.Players._Shared;
using Sports.Api.Features.Teams._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Domain.TeamAggregate.ValueObjects;

public class GetPlayersHandler(SportsDbContext db, PlayerMapper mapper, ILogger<GetPlayersHandler> logger)
    : IRequestHandler<GetPlayersQuery, ErrorOr<ImmutableList<PlayerResponse>>>
{
    public async Task<ErrorOr<ImmutableList<PlayerResponse>>> Handle(
        GetPlayersQuery query,
        CancellationToken cancellationToken)
    {
        TeamId? teamId = query.TeamId.HasValue
            ? TeamId.Create(query.TeamId.Value)
            : null;

        if (teamId is not null)
        {
            var teamExists = await db.Teams.AnyAsync(
                t => t.Id == teamId, cancellationToken);

            if (!teamExists)
                return TeamErrors.NotFound;
        }

        var players = await db.Players
            .AsNoTracking()
            .WhereIf(teamId is not null,
                p => p.TeamId == teamId!)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Retrieved {Count} players", players.Count);

        return mapper.ToResponseList(players);
    }
}
