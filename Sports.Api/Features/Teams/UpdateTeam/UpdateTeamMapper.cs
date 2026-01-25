namespace Sports.Api.Features.Teams.UpdateTeam;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class UpdateTeamMapper
{
    public partial UpdateTeamCommand ToCommand(UpdateTeamRequest request);

    [MapperIgnoreSource(nameof(Team.Players))]
    public partial UpdateTeamResponse ToResponse(Team team);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(UpdateTeamCommand.Id))]
    public partial void Apply(
        UpdateTeamCommand command,
        Team team);
}