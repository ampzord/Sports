namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class UpdateLeagueMapper
{
    public partial UpdateLeagueCommand ToCommand(UpdateLeagueRequest request);

    [MapperIgnoreSource(nameof(League.Teams))]
    public partial LeagueResponse ToResponse(League league);

    [MapperIgnoreTarget(nameof(League.Id))]
    [MapperIgnoreTarget(nameof(League.Teams))]
    [MapperIgnoreSource(nameof(UpdateLeagueCommand.Id))]
    public partial void Apply(
        UpdateLeagueCommand command,
        League league);
}