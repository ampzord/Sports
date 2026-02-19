namespace Sports.Api.Features.Teams.UpdateTeam;

using Microsoft.AspNetCore.Mvc;

public record UpdateTeamRequest(
    [FromRoute] Guid Id,
    string Name,
    Guid? LeagueId = null)
{
    public static UpdateTeamRequest Example => new(Guid.Empty, "FC Barcelona", Guid.Empty);
}
