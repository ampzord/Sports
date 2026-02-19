namespace Sports.Api.Features.Players._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Domain.Entities;

[Mapper]
public partial class PlayerMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(Player.DomainEvents))]
    public partial PlayerResponse ToResponse(Player player);

    public partial ImmutableList<PlayerResponse> ToResponseList(List<Player> players);

    public partial AddPlayerCommand ToCommand(AddPlayerRequest request);

    public partial UpdatePlayerCommand ToCommand(UpdatePlayerRequest request);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    [MapperIgnoreTarget(nameof(Player.DomainEvents))]
    public partial Player ToEntity(AddPlayerCommand command);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    [MapperIgnoreTarget(nameof(Player.DomainEvents))]
    [MapperIgnoreSource(nameof(UpdatePlayerCommand.Id))]
    public partial void Apply(UpdatePlayerCommand command, Player player);
}
