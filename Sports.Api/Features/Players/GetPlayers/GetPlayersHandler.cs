namespace Sports.Api.Features.Players.GetPlayers;

using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetPlayersHandler(SportsDbContext db, GetPlayersMapper mapper, ILogger<GetPlayersHandler> logger)
    : IRequestHandler<GetPlayersQuery, ErrorOr<List<PlayerResponse>>>
{
    public async Task<ErrorOr<List<PlayerResponse>>> Handle(
        GetPlayersQuery query,
        CancellationToken cancellationToken)
    {
        if (query.TeamId is not null)
        {
            var teamExists = await db.Teams.AnyAsync(
                t => t.Id == query.TeamId, cancellationToken);

            if (!teamExists)
                return Error.NotFound("Team.NotFound", "Team not found");
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
