namespace Sports.Api.Features.Teams.GetTeam;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class GetTeamMapper
{
    public partial GetTeamQuery ToQuery(GetTeamRequest request);

    [MapperIgnoreSource(nameof(Team.Players))]
    public partial GetTeamResponse ToResponse(Team team);
}