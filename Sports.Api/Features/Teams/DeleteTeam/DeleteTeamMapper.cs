namespace Sports.Api.Features.Teams.DeleteTeam;

using Riok.Mapperly.Abstractions;

[Mapper]
public partial class DeleteTeamMapper
{
    public partial DeleteTeamCommand ToCommand(DeleteTeamRequest request);
}