namespace Sports.Api.Features.Teams._Shared;

using FluentValidation;

public static class TeamPropertyValidator
{
    public static IRuleBuilderOptions<T, string> ValidateTeamName<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    public static IRuleBuilderOptions<T, int?> ValidateLeagueId<T>(
        this IRuleBuilder<T, int?> ruleBuilder) => ruleBuilder
            .GreaterThan(0).When(x => ruleBuilder != null)
            .WithMessage("LeagueId must be greater than 0 if provided");
}