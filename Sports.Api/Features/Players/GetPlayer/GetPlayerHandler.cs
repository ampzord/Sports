namespace Sports.Api.Features.Players.GetPlayer;

using MediatR;
using Sports.Api.Database;

public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, GetPlayerResponse?>
{
    private readonly SportsDbContext _db;
    private readonly GetPlayerMapper _mapper;

    public GetPlayerHandler(SportsDbContext db, GetPlayerMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetPlayerResponse?> Handle(
        GetPlayerQuery query,
        CancellationToken cancellationToken)
    {
        var player = await _db.Players.FindAsync(query.Id, cancellationToken);

        if (player is null)
            return null;

        return _mapper.ToResponse(player);
    }
}