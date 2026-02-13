namespace Sports.Api.Features.Leagues._Shared;

using ErrorOr;

public static class LeagueErrors
{
    public static readonly Error NotFound =
        Error.NotFound("League.NotFound", "League not found");

    public static readonly Error NameConflict =
        Error.Conflict("League.NameConflict", "A league with the same name already exists");

    public static readonly Error HasTeams =
        Error.Conflict("League.HasTeams", "Cannot delete a league that has teams");
}
