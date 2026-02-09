namespace Sports.Api.Features.Leagues.GetLeagueById;

using Sports.Api.Features.Leagues._Shared;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class GetLeagueByIdHandler(SportsDbContext db, LeagueMapper mapper)
    : IRequestHandler<GetLeagueByIdQuery, ErrorOr<LeagueResponse>>
{
    public async Task<ErrorOr<LeagueResponse>> Handle(
        GetLeagueByIdQuery query,
        CancellationToken cancellationToken)
    {
        var league = await db.Leagues.FindAsync([query.Id], cancellationToken);

        if (league is null)
            return Error.NotFound("League.NotFound", "League not found");

        return mapper.ToResponse(league);
    }
}