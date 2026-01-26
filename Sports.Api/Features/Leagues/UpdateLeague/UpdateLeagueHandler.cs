namespace Sports.Api.Features.Leagues.UpdateLeague;

using MediatR;
using Sports.Api.Database;

public class UpdateLeagueHandler : IRequestHandler<UpdateLeagueCommand, UpdateLeagueResponse?>
{
    private readonly SportsDbContext _db;
    private readonly UpdateLeagueMapper _mapper;

    public UpdateLeagueHandler(SportsDbContext db, UpdateLeagueMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UpdateLeagueResponse?> Handle(
        UpdateLeagueCommand command,
        CancellationToken cancellationToken)
    {
        var league = await _db.Leagues.FindAsync(
            new object[] { command.Id },
            cancellationToken);

        if (league is null)
            return null;

        _mapper.Apply(command, league);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.ToResponse(league);
    }
}