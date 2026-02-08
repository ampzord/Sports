namespace Sports.Api.Features.Matches.GetMatchById;

using Sports.Api.Features.Matches._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetMatchByIdMapper
{
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    public partial MatchResponse ToResponse(Match match);
}
