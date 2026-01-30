using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Leagues.UpdateLeague;

public class UpdateLeagueRequest
{
    [FromRoute]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}