namespace Sports.Api.Features.Leagues.AddLeague;

using FastEndpoints;
using Sports.Api.Features.Leagues._Shared;

public class AddLeagueValidator : Validator<AddLeagueRequest>
{
    public AddLeagueValidator() => RuleFor(x => x.Name).ValidateLeagueName();
}