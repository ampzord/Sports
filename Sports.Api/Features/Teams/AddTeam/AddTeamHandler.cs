namespace Sports.Api.Features.Teams.AddTeam;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class AddTeamHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<AddTeamCommand, ErrorOr<TeamResponse>>
{

    public async Task<ErrorOr<TeamResponse>> Handle(
        AddTeamCommand command,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var leagueExists = await db.Leagues.AnyAsync(
            l => l.Id == command.LeagueId,
            cancellationToken);

        if (!leagueExists)
            errors.Add(Error.NotFound("League.NotFound", "League not found"));

        var nameExists = await db.Teams.AnyAsync(
            t => t.Name == command.Name,
            cancellationToken);

        if (nameExists)
            errors.Add(Error.Conflict("Team.NameConflict", $"A team with the name '{command.Name}' already exists"));

        if (errors.Count > 0)
            return errors;

        var team = mapper.ToEntity(command);
        db.Teams.Add(team);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(team);
    }
}