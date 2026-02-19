namespace Sports.Api.Features.Leagues.GetLeagueById;

using Sports.Api.Features.Leagues._Shared;
using ErrorOr;
using MediatR;
using Sports.Api.Database;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class GetLeagueByIdHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<GetLeagueByIdQuery, ErrorOr<LeagueResponse>>
{
    public async Task<ErrorOr<LeagueResponse>> Handle(
        GetLeagueByIdQuery query,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync([LeagueId.Create(query.Id)], cancellationToken);

        if (league is null)
            return LeagueErrors.NotFound;

        return mapper.ToResponse(league);
    }
}
