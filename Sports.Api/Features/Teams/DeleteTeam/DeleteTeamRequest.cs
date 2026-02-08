namespace Sports.Api.Features.Teams.DeleteTeam;

public record DeleteTeamRequest(int Id)
{
    public static DeleteTeamRequest Example => new(1);
}