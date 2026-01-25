namespace Sports.Api.Features.Players.GetPlayer;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class GetPlayerMapper
{
    public partial GetPlayerQuery ToQuery(GetPlayerRequest request);
    public partial GetPlayerResponse ToResponse(Player player);
}