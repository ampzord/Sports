namespace Sports.Api.Features.Teams.AddTeam;

using MediatR;
using Sports.Api.Database;

public class AddTeamHandler : IRequestHandler<AddTeamCommand, AddTeamResponse>
{
    private readonly SportsDbContext _db;
    private readonly AddTeamMapper _mapper;

    public AddTeamHandler(SportsDbContext db, AddTeamMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<AddTeamResponse> Handle(
        AddTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = _mapper.ToEntity(command);

        _db.Teams.Add(team);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(team);
    }
}