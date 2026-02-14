namespace Sports.Api.Features.Leagues.GetLeagues;

using System.Collections.Immutable;
using Sports.Api.Features.Leagues._Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetLeaguesHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<GetLeaguesQuery, ImmutableList<LeagueResponse>>
{

    public async Task<ImmutableList<LeagueResponse>> Handle(
        GetLeaguesQuery query,
        CancellationToken cancellationToken)
    {
        var leagues = await db.Leagues
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.ToResponseList(leagues);
    }
}
