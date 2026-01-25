namespace Sports.Api.Features.Players.AddPlayer;

using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;

[Mapper]
public partial class AddPlayerMapper
{
    public partial AddPlayerCommand ToCommand(AddPlayerRequest request);
    public partial AddPlayerResponse ToResponse(Player player);

    [MapperIgnoreTarget(nameof(Player.Id))]
    public partial Player ToEntity(AddPlayerCommand command);
}