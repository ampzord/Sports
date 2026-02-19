namespace Sports.Api.Features.Leagues.UpdateLeague;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Leagues._Shared;

public class UpdateLeagueValidator : Validator<UpdateLeagueRequest>
{
    public UpdateLeagueValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(x => x.Name).ValidateLeagueName();
    }
}