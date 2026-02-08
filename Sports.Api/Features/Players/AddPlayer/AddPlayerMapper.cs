namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Features.Players._Shared.Responses;

using Riok.Mapperly.Abstractions;
using Sports.Shared.Entities;

[Mapper]
public partial class AddPlayerMapper
{
    public AddPlayerCommand ToCommand(AddPlayerRequest request)
    {
        return new AddPlayerCommand(request.Name, request.Position, request.TeamId);
    }

    [MapperIgnoreSource(nameof(Player.Team))]
    public partial PlayerResponse ToResponse(Player player);

    [MapperIgnoreTarget(nameof(Player.Id))]
    [MapperIgnoreTarget(nameof(Player.Team))]
    public partial Player ToEntity(AddPlayerCommand command);
}