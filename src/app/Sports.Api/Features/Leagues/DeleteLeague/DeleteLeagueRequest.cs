namespace Sports.Api.Features.Leagues.DeleteLeague;

public record DeleteLeagueRequest(Guid Id)
{
    public static DeleteLeagueRequest Example => new(Guid.Empty);
}