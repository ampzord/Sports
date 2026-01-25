namespace Sports.Api.Features.Teams.AddTeam;

public class AddTeamRequest
{
    public string Name { get; set; } = string.Empty;
    public int? LeagueId { get; set; }
}