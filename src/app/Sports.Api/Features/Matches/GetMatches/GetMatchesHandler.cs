namespace Sports.Api.Features.Matches.GetMatches;

using System.Collections.Immutable;
using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Matches._Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetMatchesHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<GetMatchesQuery, ErrorOr<ImmutableList<MatchResponse>>>
{
    public async Task<ErrorOr<ImmutableList<MatchResponse>>> Handle(
        GetMatchesQuery query,
        CancellationToken cancellationToken)
    {
        if (query.LeagueId is not null)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == query.LeagueId, cancellationToken);

            if (!leagueExists)
                return LeagueErrors.NotFound;
        }

        var matchesQuery = db.Matches
            .AsNoTracking()
            .AsQueryable();

        if (query.LeagueId is not null)
            matchesQuery = matchesQuery
                .Where(m => m.HomeTeam.LeagueId == query.LeagueId);

        var matches = await matchesQuery.ToListAsync(cancellationToken);

        return mapper.ToResponseList(matches);
    }
}
