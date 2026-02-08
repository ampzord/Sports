namespace Sports.Api.Features.Matches.AddMatch;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class AddMatchHandler(SportsDbContext db, AddMatchMapper mapper)
    : IRequestHandler<AddMatchCommand, ErrorOr<MatchResponse>>
{

    public async Task<ErrorOr<MatchResponse>> Handle(
        AddMatchCommand command,
        CancellationToken cancellationToken)
    {
        var homeTeamExists = await db.Teams.AnyAsync(
            t => t.Id == command.HomeTeamId, cancellationToken);

        if (!homeTeamExists)
            return Error.NotFound("HomeTeam.NotFound", "Home team not found");

        var awayTeamExists = await db.Teams.AnyAsync(
            t => t.Id == command.AwayTeamId, cancellationToken);

        if (!awayTeamExists)
            return Error.NotFound("AwayTeam.NotFound", "Away team not found");

        var match = mapper.ToEntity(command);

        db.Matches.Add(match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}