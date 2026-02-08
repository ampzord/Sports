using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Leagues.UpdateLeague;

public record UpdateLeagueRequest(
    [FromRoute] int Id,
    string Name)
{
    public static UpdateLeagueRequest Example => new(1, "La Liga");
}