namespace Sports.Api.Features.Teams._Shared;

using ErrorOr;

public static class TeamErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Team.NotFound", "Team not found");

    public static readonly Error NameConflict =
        Error.Conflict("Team.NameConflict", "A team with the same name already exists");

    public static readonly Error HasPlayers =
        Error.Conflict("Team.HasPlayers", "Cannot delete a team that has players");

    public static readonly Error HasMatches =
        Error.Conflict("Team.HasMatches", "Cannot delete a team that has matches");
}
