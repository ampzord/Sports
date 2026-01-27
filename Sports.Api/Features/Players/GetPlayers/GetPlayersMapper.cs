namespace Sports.Api.Features.Players.GetPlayers;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class GetPlayersMapper
{
    public partial List<GetPlayersResponse> ToResponseList(List<Player> players);
    public partial GetPlayersResponse ToResponse(Player player);
}