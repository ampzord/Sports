namespace Sports.Api.Features.Teams.UpdateTeam;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Teams._Shared;

public class UpdateTeamValidator : Validator<UpdateTeamRequest>
{
    public UpdateTeamValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.Name).ValidateTeamName();
        RuleFor(x => x.LeagueId).ValidateLeagueId();
    }
}