namespace Sports.Api.Features.Teams.GetTeam;

using MediatR;
using Sports.Api.Database;

public class GetTeamHandler : IRequestHandler<GetTeamQuery, GetTeamResponse?>
{
    private readonly SportsDbContext _db;
    private readonly GetTeamMapper _mapper;

    public GetTeamHandler(SportsDbContext db, GetTeamMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetTeamResponse?> Handle(
        GetTeamQuery query,
        CancellationToken cancellationToken)
    {
        var team = await _db.Teams.FindAsync(
            new object[] { query.Id },
            cancellationToken);

        if (team is null)
            return null;

        return _mapper.ToResponse(team);
    }
}