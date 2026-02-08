namespace Sports.Api.Features.Teams.GetTeamById;

using Sports.Api.Features.Teams._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetTeamByIdMapper
{
    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    public partial TeamResponse ToResponse(Team team);
}