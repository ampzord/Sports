namespace Sports.Api.Features.Players.GetPlayers;

using Sports.Api.Features.Players._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetPlayersMapper
{
    public partial List<PlayerResponse> ToResponseList(List<Player> players);

    [MapperIgnoreSource(nameof(Player.Team))]
    public partial PlayerResponse ToResponse(Player player);
}
