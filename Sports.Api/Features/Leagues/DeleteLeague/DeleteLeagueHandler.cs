namespace Sports.Api.Features.Leagues.DeleteLeague;

using MediatR;
using Sports.Api.Database;

public class DeleteLeagueHandler : IRequestHandler<DeleteLeagueCommand, DeleteLeagueResponse>
{
    private readonly SportsDbContext _db;

    public DeleteLeagueHandler(SportsDbContext db) => _db = db;

    public async Task<DeleteLeagueResponse> Handle(
        DeleteLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await _db.Leagues.FindAsync(
            new object[] { command.Id },
            cancellationToken);

        if (league is null)
            return new DeleteLeagueResponse { Success = false };

        _db.Leagues.Remove(league);
        await _db.SaveChangesAsync(cancellationToken);

        return new DeleteLeagueResponse { Success = true };
    }
}