namespace Sports.Api.Features.Matches.UpdateMatch;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class UpdateMatchMapper
{
    public partial UpdateMatchCommand ToCommand(UpdateMatchRequest request);
    public partial UpdateMatchResponse ToResponse(Match match);

    [MapperIgnoreTarget(nameof(Match.Id))]
    [MapperIgnoreSource(nameof(UpdateMatchCommand.Id))]
    public partial void Apply(
        UpdateMatchCommand command,
        Match match);
}