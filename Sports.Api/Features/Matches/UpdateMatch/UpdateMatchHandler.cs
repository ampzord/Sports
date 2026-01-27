namespace Sports.Api.Features.Matches.UpdateMatch;

using MediatR;
using Sports.Api.Database;

public class UpdateMatchHandler : IRequestHandler<UpdateMatchCommand, UpdateMatchResponse?>
{
    private readonly SportsDbContext _db;
    private readonly UpdateMatchMapper _mapper;

    public UpdateMatchHandler(SportsDbContext db, UpdateMatchMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UpdateMatchResponse?> Handle(
        UpdateMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await _db.Matches.FindAsync(command.Id, cancellationToken);

        if (match is null)
            return null;

        _mapper.Apply(command, match);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(match);
    }
}