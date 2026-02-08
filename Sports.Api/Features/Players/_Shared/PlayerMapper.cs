namespace Sports.Api.Features.Players._Shared;

using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Players._Shared.Responses;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Shared.Entities;

[Mapper]
public partial class PlayerMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    public partial PlayerResponse ToResponse(Player player);

    public partial List<PlayerResponse> ToResponseList(List<Player> players);

    public partial AddPlayerCommand ToCommand(AddPlayerRequest request);

    public partial UpdatePlayerCommand ToCommand(UpdatePlayerRequest request);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    public partial Player ToEntity(AddPlayerCommand command);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(UpdatePlayerCommand.Id))]
    public partial void Apply(UpdatePlayerCommand command, Player player);
}
