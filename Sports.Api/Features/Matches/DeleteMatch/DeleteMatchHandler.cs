namespace Sports.Api.Features.Matches.DeleteMatch;

using MediatR;
using Sports.Api.Database;

public class DeleteMatchHandler : IRequestHandler<DeleteMatchCommand, DeleteMatchResponse>
{
    private readonly SportsDbContext _db;

    public DeleteMatchHandler(SportsDbContext db) => _db = db;

    public async Task<DeleteMatchResponse> Handle(
        DeleteMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await _db.Matches.FindAsync(command.Id, cancellationToken);

        if (match is null)
            return new DeleteMatchResponse { Success = false };

        _db.Matches.Remove(match);
        await _db.SaveChangesAsync(cancellationToken);

        return new DeleteMatchResponse { Success = true };
    }
}