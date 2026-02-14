namespace Sports.Api.Features.Leagues.UpdateLeague;

using Microsoft.AspNetCore.Mvc;

public record UpdateLeagueRequest(
    [FromRoute] int Id,
    string Name)
{
    public static UpdateLeagueRequest Example => new(1, "La Liga");
}
