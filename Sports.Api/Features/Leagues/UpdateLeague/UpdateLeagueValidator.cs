namespace Sports.Api.Features.Leagues.UpdateLeague;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Leagues._Shared;

public class UpdateLeagueValidator : Validator<UpdateLeagueRequest>
{
    public UpdateLeagueValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.Name).ValidateLeagueName();
    }
}