namespace Sports.Api.Features.Matches.AddMatch;

using Sports.Api.Features.Matches._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class AddMatchMapper
{
    public partial AddMatchCommand ToCommand(AddMatchRequest request);
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    public partial MatchResponse ToResponse(Match match);

    [MapperIgnoreTarget(nameof(Match.Id))]
    [MapperIgnoreTarget(nameof(Match.HomeTeam))]
    [MapperIgnoreTarget(nameof(Match.AwayTeam))]
    [MapperIgnoreSource(nameof(AddMatchCommand.LeagueId))]
    public partial Match ToEntity(AddMatchCommand command);
}
