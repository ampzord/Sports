namespace Sports.Api.Features.Matches.AddMatch;

using MediatR;
using Sports.Api.Database;

public class AddMatchHandler : IRequestHandler<AddMatchCommand, AddMatchResponse>
{
    private readonly SportsDbContext _db;
    private readonly AddMatchMapper _mapper;

    public AddMatchHandler(SportsDbContext db, AddMatchMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<AddMatchResponse> Handle(
        AddMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = _mapper.ToEntity(command);

        _db.Matches.Add(match);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(match);
    }
}