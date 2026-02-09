namespace Sports.Api.Features.Matches._Shared;

using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Matches.AddMatch;
using Sports.Api.Features.Matches.DeleteMatch;
using Sports.Api.Features.Matches.UpdateMatch;
using Sports.Shared.Entities;

[Mapper]
public partial class MatchMapper
{
    [MapperIgnoreSource(nameof(Match.HomeTeam))]
    [MapperIgnoreSource(nameof(Match.AwayTeam))]
    public partial MatchResponse ToResponse(Match match);

    public partial List<MatchResponse> ToResponseList(List<Match> matches);

    public partial AddMatchCommand ToCommand(AddMatchRequest request);

    public partial UpdateMatchCommand ToCommand(UpdateMatchRequest request);

    public partial DeleteMatchCommand ToCommand(DeleteMatchRequest request);

    [MapperIgnoreTarget(nameof(Match.Id))]
    [MapperIgnoreTarget(nameof(Match.HomeTeam))]
    [MapperIgnoreTarget(nameof(Match.AwayTeam))]
    [MapperIgnoreSource(nameof(AddMatchCommand.LeagueId))]
    public partial Match ToEntity(AddMatchCommand command);

    [MapperIgnoreTarget(nameof(Match.Id))]
    [MapperIgnoreTarget(nameof(Match.HomeTeam))]
    [MapperIgnoreTarget(nameof(Match.AwayTeam))]
    [MapperIgnoreSource(nameof(UpdateMatchCommand.Id))]
    public partial void Apply(UpdateMatchCommand command, Match match);
}
