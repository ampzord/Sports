using Riok.Mapperly.Abstractions;
using Sports.Api.Entities;
using Sports.Api.Features.Players.UpdatePlayer;

[Mapper]
public partial class UpdatePlayerMapper
{
    public partial UpdatePlayerCommand ToCommand(UpdatePlayerRequest request);

    public partial UpdatePlayerResponse ToResponse(Player player);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreSource(nameof(UpdatePlayerCommand.Id))]
    public partial void Apply(
        UpdatePlayerCommand command,
        Player player);
}