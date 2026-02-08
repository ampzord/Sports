using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Teams.UpdateTeam;

public record UpdateTeamRequest(
    [FromRoute] int Id,
    string Name,
    int? LeagueId = null)
{
    public static UpdateTeamRequest Example => new(1, "FC Barcelona", 1);
}