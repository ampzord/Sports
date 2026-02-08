namespace Sports.Api.Features.Leagues.AddLeague;

using Sports.Api.Features.Leagues._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class AddLeagueMapper
{
    public partial AddLeagueCommand ToCommand(AddLeagueRequest request);

    [MapperIgnoreSource(nameof(League.Teams))]
    public partial LeagueResponse ToResponse(League league);

    [MapperIgnoreTarget(nameof(League.Id))]
    [MapperIgnoreTarget(nameof(League.Teams))]
    public partial League ToEntity(AddLeagueCommand command);
}