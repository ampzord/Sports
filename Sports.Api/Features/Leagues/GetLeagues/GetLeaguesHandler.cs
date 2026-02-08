namespace Sports.Api.Features.Leagues.GetLeagues;

using Sports.Api.Features.Leagues._Shared;
using Sports.Api.Features.Leagues._Shared.Responses;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetLeaguesHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<GetLeaguesQuery, List<LeagueResponse>>
{

    public async Task<List<LeagueResponse>> Handle(
        GetLeaguesQuery query,
        CancellationToken cancellationToken)
    {
        var leagues = await db.Leagues
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.ToResponseList(leagues);
    }
}