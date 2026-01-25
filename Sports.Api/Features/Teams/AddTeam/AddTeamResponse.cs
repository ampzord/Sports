namespace Sports.Api.Features.Teams.AddTeam;

public class AddTeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? LeagueId { get; set; }
}