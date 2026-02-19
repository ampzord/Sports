namespace Sports.Api.Features.Matches._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Domain.MatchAggregate;
using Sports.Domain.MatchAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

[Mapper]
public partial class MatchMapper
{
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    [MapperIgnoreSource(nameof(Match.DomainEvents))]
    public partial MatchResponse ToResponse(Match match);

    public partial ImmutableList<MatchResponse> ToResponseList(List<Match> matches);

    public partial AddMatchCommand ToCommand(AddMatchRequest request);

    public partial UpdateMatchCommand ToCommand(UpdateMatchRequest request);

    public partial DeleteMatchCommand ToCommand(DeleteMatchRequest request);

    private static Guid ToGuid(MatchId id) => id.Value;
    private static Guid ToGuid(TeamId id) => id.Value;
}
