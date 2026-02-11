namespace Sports.Api.Features.Teams.GetTeams;

using System.Collections.Immutable;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Teams._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetTeamsHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<GetTeamsQuery, ErrorOr<ImmutableList<TeamResponse>>>
{
    public async Task<ErrorOr<ImmutableList<TeamResponse>>> Handle(
        GetTeamsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.LeagueId is not null)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == query.LeagueId, cancellationToken);

            if (!leagueExists)
                return LeagueErrors.NotFound;
        }

        var teams = await db.Teams
            .AsNoTracking()
            .WhereIf(query.LeagueId is not null,
                t => t.LeagueId == query.LeagueId)
            .ToListAsync(cancellationToken);

        return mapper.ToResponseList(teams);
    }
}
