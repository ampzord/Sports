namespace Sports.Api.Features.Leagues.DeleteLeague;

using Riok.Mapperly.Abstractions;

[Mapper]
public partial class DeleteLeagueMapper
{
    public partial DeleteLeagueCommand ToCommand(DeleteLeagueRequest request);
}