namespace Sports.Api.Features.Matches.GetMatches;

using Sports.Api.Features.Matches._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetMatchesMapper
{
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    private partial MatchResponse ToResponse(Match match);

    public List<MatchResponse> ToResponseList(List<Match> matches)
    {
        return matches.Select(ToResponse).ToList();
    }
}
