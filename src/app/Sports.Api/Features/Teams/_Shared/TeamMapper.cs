namespace Sports.Api.Features.Teams._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.UpdateTeam;
using Sports.Domain.Entities;

[Mapper]
public partial class TeamMapper
{
    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    public partial TeamResponse ToResponse(Team team);

    public partial ImmutableList<TeamResponse> ToResponseList(List<Team> teams);

    public partial AddTeamCommand ToCommand(AddTeamRequest request);

    public partial UpdateTeamCommand ToCommand(UpdateTeamRequest request);

    public partial DeleteTeamCommand ToCommand(DeleteTeamRequest request);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreTarget(nameof(Team.League))]
    public partial Team ToEntity(AddTeamCommand command);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreTarget(nameof(Team.League))]
    [MapperIgnoreSource(nameof(UpdateTeamCommand.Id))]
    public partial void Apply(UpdateTeamCommand command, Team team);
}
