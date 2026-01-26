namespace Sports.Api.Features.Matches.GetMatch;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class GetMatchMapper
{
    public partial GetMatchQuery ToQuery(GetMatchRequest request);
    public partial GetMatchResponse ToResponse(Match match);
}