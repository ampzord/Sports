namespace Sports.Api.Features.Teams._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Teams.AddTeam;
using Sports.Api.Features.Teams.DeleteTeam;
using Sports.Api.Features.Teams.UpdateTeam;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

[Mapper]
public partial class TeamMapper
{
    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    [MapperIgnoreSource(nameof(Team.DomainEvents))]
    public partial TeamResponse ToResponse(Team team);

    public partial ImmutableList<TeamResponse> ToResponseList(List<Team> teams);

    public partial AddTeamCommand ToCommand(AddTeamRequest request);

    public partial UpdateTeamCommand ToCommand(UpdateTeamRequest request);

    public partial DeleteTeamCommand ToCommand(DeleteTeamRequest request);

    private static Guid ToGuid(TeamId id) => id.Value;
    private static Guid ToGuid(LeagueId id) => id.Value;
}
