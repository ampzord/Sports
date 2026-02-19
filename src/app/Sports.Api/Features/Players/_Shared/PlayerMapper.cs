namespace Sports.Api.Features.Players._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Players.AddPlayer;
using Sports.Api.Features.Players.UpdatePlayer;
using Sports.Domain.PlayerAggregate;
using Sports.Domain.PlayerAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

[Mapper]
public partial class PlayerMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(Player.DomainEvents))]
    public partial PlayerResponse ToResponse(Player player);

    public partial ImmutableList<PlayerResponse> ToResponseList(List<Player> players);

    public partial AddPlayerCommand ToCommand(AddPlayerRequest request);

    public partial UpdatePlayerCommand ToCommand(UpdatePlayerRequest request);

    private static Guid ToGuid(PlayerId id) => id.Value;
    private static Guid ToGuid(TeamId id) => id.Value;
}
