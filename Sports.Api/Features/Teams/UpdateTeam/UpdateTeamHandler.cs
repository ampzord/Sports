namespace Sports.Api.Features.Teams.UpdateTeam;

using MediatR;
using Sports.Api.Database;

public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, UpdateTeamResponse?>
{
    private readonly SportsDbContext _db;
    private readonly UpdateTeamMapper _mapper;

    public UpdateTeamHandler(SportsDbContext db, UpdateTeamMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UpdateTeamResponse?> Handle(
        UpdateTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = await _db.Teams.FindAsync(
            new object[] { command.Id },
            cancellationToken);

        if (team is null)
            return null;

        _mapper.Apply(command, team);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(team);
    }
}