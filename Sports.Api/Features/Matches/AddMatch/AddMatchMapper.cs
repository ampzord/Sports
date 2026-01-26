namespace Sports.Api.Features.Matches.AddMatch;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class AddMatchMapper
{
    public partial AddMatchCommand ToCommand(AddMatchRequest request);
    public partial AddMatchResponse ToResponse(Match match);

    [MapperIgnoreTarget(nameof(Match.Id))]
    public partial Match ToEntity(AddMatchCommand command);
}