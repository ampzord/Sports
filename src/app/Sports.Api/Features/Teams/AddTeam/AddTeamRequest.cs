namespace Sports.Api.Features.Teams.AddTeam;

public record AddTeamRequest(
    Guid LeagueId,
    string Name)
{
    public static AddTeamRequest Example => new(Guid.Empty, "FC Barcelona");
}