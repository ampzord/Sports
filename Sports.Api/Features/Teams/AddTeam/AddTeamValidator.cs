namespace Sports.Api.Features.Teams.AddTeam;

using FastEndpoints;
using Sports.Api.Features.Teams._Shared;

public class AddTeamValidator : Validator<AddTeamRequest>
{
    public AddTeamValidator()
    {
        RuleFor(x => x.Name).ValidateTeamName();
        RuleFor(x => x.LeagueId).ValidateLeagueId();
    }
}