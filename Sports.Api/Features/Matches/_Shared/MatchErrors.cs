namespace Sports.Api.Features.Matches._Shared;

using ErrorOr;

public static class MatchErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Match.NotFound", "Match not found");

    public static readonly Error HomeTeamNotFound =
        Error.NotFound("HomeTeam.NotFound", "Home team not found");

    public static readonly Error AwayTeamNotFound =
        Error.NotFound("AwayTeam.NotFound", "Away team not found");

    public static readonly Error DifferentLeagues =
        Error.Validation("Match.DifferentLeagues", "Both teams must belong to the same league");

    public static readonly Error LeagueMismatch =
        Error.Validation("Match.LeagueMismatch", "Teams do not belong to the specified league");
}
