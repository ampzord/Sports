namespace Sports.Api.Features.Matches._Shared;

using FluentValidation;

public static class MatchPropertyValidator
{
    public static IRuleBuilderOptions<T, int> ValidateTeamId<T>(
        this IRuleBuilder<T, int> ruleBuilder) => ruleBuilder
            .GreaterThan(0).WithMessage("TeamId must be greater than 0");
}