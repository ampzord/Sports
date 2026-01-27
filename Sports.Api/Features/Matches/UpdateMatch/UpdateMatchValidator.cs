namespace Sports.Api.Features.Matches.UpdateMatch;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Matches._Shared;

public class UpdateMatchValidator : Validator<UpdateMatchRequest>
{
    public UpdateMatchValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.HomeTeamId).ValidateTeamId();
        RuleFor(x => x.AwayTeamId).ValidateTeamId();
        RuleFor(x => x.AwayTeamId)
            .NotEqual(x => x.HomeTeamId)
            .WithMessage("Home team and away team must be different");
    }
}