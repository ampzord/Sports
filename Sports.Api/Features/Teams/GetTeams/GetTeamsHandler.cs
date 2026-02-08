namespace Sports.Api.Features.Teams.GetTeams;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetTeamsHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<GetTeamsQuery, ErrorOr<List<TeamResponse>>>
{
    public async Task<ErrorOr<List<TeamResponse>>> Handle(
        GetTeamsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.LeagueId is not null)
        {
            var leagueExists = await db.Leagues.AnyAsync(
                l => l.Id == query.LeagueId, cancellationToken);

            if (!leagueExists)
                return Error.NotFound("League.NotFound", "League not found");
        }

        var teams = await db.Teams
            .AsNoTracking()
            .WhereIf(query.LeagueId is not null,
                t => t.LeagueId == query.LeagueId)
            .ToListAsync(cancellationToken);

        return mapper.ToResponseList(teams);
    }
}
