using Sports.Api.Features.Teams._Shared.Responses;
using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;
using Sports.Api.Features.Teams.UpdateTeam;

[Mapper]
public partial class UpdateTeamMapper
{
    public UpdateTeamCommand ToCommand(UpdateTeamRequest request)
    {
        return new UpdateTeamCommand(
            request.Id,
            request.Name,
            request.LeagueId);
    }

    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    public partial TeamResponse ToResponse(Team team);

    [MapperIgnoreTarget(nameof(Team.Id))]
    [MapperIgnoreTarget(nameof(Team.Players))]
    [MapperIgnoreTarget(nameof(Team.League))]
    [MapperIgnoreSource(nameof(UpdateTeamCommand.Id))]
    public partial void Apply(
        UpdateTeamCommand command,
        Team team);
}