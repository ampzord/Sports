namespace Sports.Api.Features.Leagues.GetLeague;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class GetLeagueMapper
{
    public partial GetLeagueQuery ToQuery(GetLeagueRequest request);

    [MapperIgnoreSource(nameof(League.Teams))]
    public partial GetLeagueResponse ToResponse(League league);
}