namespace Sports.Api.Features.Players.GetPlayers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class GetPlayersHandler : IRequestHandler<GetPlayersQuery, List<GetPlayersResponse>>
{
    private readonly SportsDbContext _db;
    private readonly GetPlayersMapper _mapper;
    private readonly ILogger<GetPlayersHandler> _logger;

    public GetPlayersHandler(
        SportsDbContext db,
        GetPlayersMapper mapper,
        ILogger<GetPlayersHandler> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<GetPlayersResponse>> Handle(
        GetPlayersQuery query,
        CancellationToken cancellationToken)
    {
        var players = await _db.Players.ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} players", players.Count);

        return _mapper.ToResponseList(players);
    }
}