namespace Sports.Api.Features.Matches.GetMatch;

using MediatR;
using Sports.Api.Database;

public class GetMatchHandler : IRequestHandler<GetMatchQuery, GetMatchResponse?>
{
    private readonly SportsDbContext _db;
    private readonly GetMatchMapper _mapper;

    public GetMatchHandler(SportsDbContext db, GetMatchMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetMatchResponse?> Handle(
        GetMatchQuery query,
        CancellationToken cancellationToken)
    {
        var match = await _db.Matches.FindAsync(
            new object[] { query.Id },
            cancellationToken);

        if (match is null)
            return null;

        return _mapper.ToResponse(match);
    }
}