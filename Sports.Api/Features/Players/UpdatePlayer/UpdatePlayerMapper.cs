using Sports.Api.Features.Players._Shared.Responses;
using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;
using Sports.Api.Features.Players.UpdatePlayer;

[Mapper]
public partial class UpdatePlayerMapper
{
    public partial UpdatePlayerCommand ToCommand(UpdatePlayerRequest request);

    [MapperIgnoreSource(nameof(Player.Team))]
    public partial PlayerResponse ToResponse(Player player);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(UpdatePlayerCommand.Id))]
    public partial void Apply(
        UpdatePlayerCommand command,
        Player player);
}