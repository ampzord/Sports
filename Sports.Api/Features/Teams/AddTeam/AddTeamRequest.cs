namespace Sports.Api.Features.Teams.AddTeam;

public record AddTeamRequest(
    int LeagueId,
    string Name)
{
    public static AddTeamRequest Example => new(1, "FC Barcelona");
}