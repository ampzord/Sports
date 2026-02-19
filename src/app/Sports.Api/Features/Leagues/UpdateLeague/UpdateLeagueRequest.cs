namespace Sports.Api.Features.Leagues.UpdateLeague;

using Microsoft.AspNetCore.Mvc;

public record UpdateLeagueRequest(
    [FromRoute] Guid Id,
    string Name)
{
    public static UpdateLeagueRequest Example => new(Guid.Empty, "La Liga");
}
