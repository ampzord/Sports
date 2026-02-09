namespace Sports.Api.Features.Leagues._Shared;

using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Shared.Entities;

[Mapper]
public partial class LeagueMapper
{
    [MapperIgnoreSource(nameof(League.Teams))]
    public partial LeagueResponse ToResponse(League league);

    public partial List<LeagueResponse> ToResponseList(List<League> leagues);

    public partial AddLeagueCommand ToCommand(AddLeagueRequest request);

    public partial UpdateLeagueCommand ToCommand(UpdateLeagueRequest request);

    public partial DeleteLeagueCommand ToCommand(DeleteLeagueRequest request);

    [MapperIgnoreTarget(nameof(League.Id))]
    [MapperIgnoreTarget(nameof(League.Teams))]
    public partial League ToEntity(AddLeagueCommand command);

    [MapperIgnoreTarget(nameof(League.Id))]
    [MapperIgnoreTarget(nameof(League.Teams))]
    [MapperIgnoreSource(nameof(UpdateLeagueCommand.Id))]
    public partial void Apply(UpdateLeagueCommand command, League league);
}
