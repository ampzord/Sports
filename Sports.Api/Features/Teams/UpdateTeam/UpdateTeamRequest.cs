namespace Sports.Api.Features.Teams.UpdateTeam;

public class UpdateTeamRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? LeagueId { get; set; }
}