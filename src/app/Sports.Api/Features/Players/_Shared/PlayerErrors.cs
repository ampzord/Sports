namespace Sports.Api.Features.Players._Shared;

using ErrorOr;

public static class PlayerErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Player.NotFound", "Player not found");

    public static readonly Error NameConflict =
        Error.Conflict("Player.NameConflict", "A player with the same name already exists");
}
