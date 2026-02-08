namespace Sports.Api.Features.Teams.AddTeam;

using Sports.Api.Features.Teams._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class AddTeamMapper
{
    public AddTeamCommand ToCommand(AddTeamRequest request)
    {
        return new AddTeamCommand(request.Name, request.LeagueId);
    }

    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    public partial TeamResponse ToResponse(Team team);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreTarget(nameof(Team.League))]
    public partial Team ToEntity(AddTeamCommand command);
}