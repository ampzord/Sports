namespace Sports.Api.Features.Leagues.DeleteLeague;

public record DeleteLeagueRequest(int Id)
{
    public static DeleteLeagueRequest Example => new(1);
}