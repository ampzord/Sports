namespace Sports.Api.Features.Players.AddPlayer;

using MediatR;
using Sports.Api.Database;

public class AddPlayerHandler : IRequestHandler<AddPlayerCommand, AddPlayerResponse>
{
    private readonly SportsDbContext _db;
    private readonly AddPlayerMapper _mapper;

    public AddPlayerHandler(SportsDbContext db, AddPlayerMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<AddPlayerResponse> Handle(
        AddPlayerCommand command,
        CancellationToken cancellationToken)
    {
        var player = _mapper.ToEntity(command);

        _db.Players.Add(player);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(player);
    }
}