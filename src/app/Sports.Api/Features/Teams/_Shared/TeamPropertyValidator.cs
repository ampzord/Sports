namespace Sports.Api.Features.Teams._Shared;

using FluentValidation;

public static class TeamPropertyValidator
{
    public static IRuleBuilderOptions<T, string> ValidateTeamName<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    public static IRuleBuilderOptions<T, Guid?> ValidateLeagueId<T>(
        this IRuleBuilder<T, Guid?> ruleBuilder) => ruleBuilder
            .NotEmpty().When(x => ruleBuilder != null)
            .WithMessage("LeagueId must not be empty if provided");
}