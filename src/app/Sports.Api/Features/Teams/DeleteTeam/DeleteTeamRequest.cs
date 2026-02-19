namespace Sports.Api.Features.Teams.DeleteTeam;

public record DeleteTeamRequest(Guid Id)
{
    public static DeleteTeamRequest Example => new(Guid.Empty);
}