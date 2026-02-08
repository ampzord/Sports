namespace Sports.Api.Features.Leagues.GetLeagueById;

using Sports.Api.Features.Leagues._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetLeagueByIdMapper
{
    [MapperIgnoreSource(nameof(League.Teams))]
    public partial LeagueResponse ToResponse(League league);
}