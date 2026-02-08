namespace Sports.Api.Features.Players.GetPlayerById;

using Sports.Api.Features.Players._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class GetPlayerByIdMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    public partial PlayerResponse ToResponse(Player player);
}