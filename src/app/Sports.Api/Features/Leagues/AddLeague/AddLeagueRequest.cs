namespace Sports.Api.Features.Leagues.AddLeague;

public record AddLeagueRequest(string Name)
{
    public static AddLeagueRequest Example => new("La Liga");
}