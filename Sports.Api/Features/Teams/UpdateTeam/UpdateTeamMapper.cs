using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;
using Sports.Api.Features.Teams.UpdateTeam;

[Mapper]
public partial class UpdateTeamMapper
{
    public UpdateTeamCommand ToCommand(UpdateTeamRequest request)
    {
        return new UpdateTeamCommand(
            request.Id,
            request.Name,
            request.LeagueId);
    }

    [MapperIgnoreSource(nameof(Team.Players))]
    public partial UpdateTeamResponse ToResponse(Team team);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(UpdateTeamCommand.Id))]
    public partial void Apply(
        UpdateTeamCommand command,
        Team team);
}