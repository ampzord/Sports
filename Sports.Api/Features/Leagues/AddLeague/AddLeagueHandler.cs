namespace Sports.Api.Features.Leagues.AddLeague;

using MediatR;
using Sports.Api.Database;

public class AddLeagueHandler : IRequestHandler<AddLeagueCommand, AddLeagueResponse>
{
    private readonly SportsDbContext _db;
    private readonly AddLeagueMapper _mapper;

    public AddLeagueHandler(SportsDbContext db, AddLeagueMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<AddLeagueResponse> Handle(
        AddLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = _mapper.ToEntity(command);

        _db.Leagues.Add(league);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(league);
    }
}