namespace Sports.Api.Features.Teams.GetTeam;

public class GetTeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? LeagueId { get; set; }
}