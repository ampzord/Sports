namespace Sports.Api.Features.Players.DeletePlayer;

using Riok.Mapperly.Abstractions;

[Mapper]
public partial class DeletePlayerMapper
{
    public partial DeletePlayerCommand ToCommand(DeletePlayerRequest request);
}