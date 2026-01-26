namespace Sports.Api.Features.Leagues.UpdateLeague;

public class UpdateLeagueRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}