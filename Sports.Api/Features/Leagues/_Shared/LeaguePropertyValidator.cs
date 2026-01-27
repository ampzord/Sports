namespace Sports.Api.Features.Leagues._Shared;

using FluentValidation;

public static class LeaguePropertyValidator
{
    public static IRuleBuilderOptions<T, string> ValidateLeagueName<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
}