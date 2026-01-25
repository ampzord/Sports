namespace Sports.Api.Features.Teams.DeleteTeam;

using MediatR;
using Sports.Api.Database;

public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, DeleteTeamResponse>
{
    private readonly SportsDbContext _db;

    public DeleteTeamHandler(SportsDbContext db) => _db = db;

    public async Task<DeleteTeamResponse> Handle(
        DeleteTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = await _db.Teams.FindAsync(
            new object[] { command.Id },
            cancellationToken);

        if (team is null)
            return new DeleteTeamResponse { Success = false };

        _db.Teams.Remove(team);
        await _db.SaveChangesAsync(cancellationToken);

        return new DeleteTeamResponse { Success = true };
    }
}