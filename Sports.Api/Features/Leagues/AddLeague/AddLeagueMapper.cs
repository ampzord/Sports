namespace Sports.Api.Features.Leagues.AddLeague;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class AddLeagueMapper
{
    public partial AddLeagueCommand ToCommand(AddLeagueRequest request);

    [MapperIgnoreSource(nameof(League.Teams))]
    public partial AddLeagueResponse ToResponse(League league);

    [MapperIgnoreTarget(nameof(League.Id))]
    [MapperIgnoreTarget(nameof(League.Teams))]
    public partial League ToEntity(AddLeagueCommand command);
}