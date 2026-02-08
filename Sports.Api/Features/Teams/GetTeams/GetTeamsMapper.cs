namespace Sports.Api.Features.Teams.GetTeams;

using Sports.Api.Features.Teams._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetTeamsMapper
{
    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.League))]
    private partial TeamResponse ToResponse(Team team);

    public List<TeamResponse> ToResponseList(List<Team> teams)
    {
        return teams.Select(ToResponse).ToList();
    }
}
