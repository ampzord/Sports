namespace Sports.Api.Features.Leagues.GetLeagues;

using Sports.Api.Features.Leagues._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetLeaguesMapper
{
    [MapperIgnoreSource(nameof(League.Teams))]
    private partial LeagueResponse ToResponse(League league);

    public List<LeagueResponse> ToResponseList(
        List<League> leagues)
    {
        return leagues.Select(ToResponse).ToList();
    }
}