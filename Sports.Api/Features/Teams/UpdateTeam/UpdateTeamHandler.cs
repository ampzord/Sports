namespace Sports.Api.Features.Teams.UpdateTeam;

using Sports.Api.Features.Teams._Shared;
using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public class UpdateTeamHandler(SportsDbContext db, TeamMapper mapper)
    : IRequestHandler<UpdateTeamCommand, ErrorOr<TeamResponse>>
{
    public async Task<ErrorOr<TeamResponse>> Handle(
        UpdateTeamCommand command,
        CancellationToken cancellationToken)
    {
        var team = await db.Teams.FindAsync([command.Id], cancellationToken);

        if (team is null)
            return Error.NotFound("Team.NotFound", "Team not found");

        var nameExists = await db.Teams.AnyAsync(
            t => t.Name == command.Name && t.Id != command.Id,
            cancellationToken);

        if (nameExists)
            return Error.Conflict("Team.NameConflict", $"A team with the name '{command.Name}' already exists");

        mapper.Apply(command, team);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(team);
    }
}