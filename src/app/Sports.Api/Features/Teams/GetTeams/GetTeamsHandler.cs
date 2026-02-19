namespace Sports.Api.Features.Teams.GetTeams;

using System.Collections.Immutable;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Teams._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class GetTeamsHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<GetTeamsQuery, ErrorOr<ImmutableList<TeamResponse>>>
{
    public async Task<ErrorOr<ImmutableList<TeamResponse>>> Handle(
        GetTeamsQuery query,
        CancellationToken cancellationToken)
    {
        LeagueId? leagueId = query.LeagueId.HasValue
            ? LeagueId.Create(query.LeagueId.Value)
            : null;

        if (leagueId is not null)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == leagueId, cancellationToken);

            if (!leagueExists)
                return LeagueErrors.NotFound;
        }

        var teams = await db.Teams
            .AsNoTracking()
            .WhereIf(leagueId is not null,
                t => t.LeagueId == leagueId!)
            .ToListAsync(cancellationToken);

        return mapper.ToResponseList(teams);
    }
}
