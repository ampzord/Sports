namespace Sports.Api.Features.Matches.GetMatches;

using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches._Shared.Responses;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetMatchesHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<GetMatchesQuery, ErrorOr<List<MatchResponse>>>
{
    public async Task<ErrorOr<List<MatchResponse>>> Handle(
        GetMatchesQuery query,
        CancellationToken cancellationToken)
    {
        if (query.LeagueId is not null)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == query.LeagueId, cancellationToken);

            if (!leagueExists)
                return Error.NotFound("League.NotFound", "League not found");
        }

        var matchesQuery = db.Matches
            .AsNoTracking()
            .AsQueryable();

        if (query.LeagueId is not null)
        {
            matchesQuery = matchesQuery
                .Where(m => m.HomeTeam.LeagueId == query.LeagueId);
        }

        var matches = await matchesQuery.ToListAsync(cancellationToken);

        return mapper.ToResponseList(matches);
    }
}
