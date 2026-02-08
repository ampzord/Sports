namespace Sports.Api.Features.Matches.UpdateMatch;

using Sports.Api.Features.Matches._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class UpdateMatchMapper
{
    public partial UpdateMatchCommand ToCommand(UpdateMatchRequest request);
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    public partial MatchResponse ToResponse(Match match);

    [MapperIgnoreTarget(nameof(Match.Id))]
    [MapperIgnoreTarget(nameof(Match.HomeTeam))]
    [MapperIgnoreTarget(nameof(Match.AwayTeam))]
    [MapperIgnoreSource(nameof(UpdateMatchCommand.Id))]
    public partial void Apply(
        UpdateMatchCommand command,
        Match match);
}