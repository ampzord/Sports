namespace Sports.Api.Features.Leagues.GetLeague;

using MediatR;
using Sports.Api.Database;

public class GetLeagueHandler : IRequestHandler<GetLeagueQuery, GetLeagueResponse?>
{
    private readonly SportsDbContext _db;
    private readonly GetLeagueMapper _mapper;

    public GetLeagueHandler(SportsDbContext db, GetLeagueMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetLeagueResponse?> Handle(
        GetLeagueQuery query,
        CancellationToken cancellationToken)
    {
        var league = await _db.Leagues.FindAsync(
            new object[] { query.Id },
            cancellationToken);

        if (league is null)
            return null;

        return _mapper.ToResponse(league);
    }
}