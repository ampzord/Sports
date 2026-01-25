namespace Sports.Api.Features.Teams.AddTeam;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class AddTeamMapper
{
    public partial AddTeamCommand ToCommand(AddTeamRequest request);

    [MapperIgnoreSource(nameof(Team.Players))]
    public partial AddTeamResponse ToResponse(Team team);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    public partial Team ToEntity(AddTeamCommand command);
}