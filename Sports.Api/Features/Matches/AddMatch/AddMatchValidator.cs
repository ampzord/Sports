namespace Sports.Api.Features.Matches.AddMatch;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Matches._Shared;

public class AddMatchValidator : Validator<AddMatchRequest>
{
    public AddMatchValidator()
    {
        RuleFor(x => x.HomeTeamId).ValidateTeamId();
        RuleFor(x => x.AwayTeamId).ValidateTeamId();
        RuleFor(x => x.AwayTeamId)
            .NotEqual(x => x.HomeTeamId)
            .WithMessage("Home team and away team must be different");
    }
}