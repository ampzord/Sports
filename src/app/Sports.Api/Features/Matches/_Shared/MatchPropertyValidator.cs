namespace Sports.Api.Features.Matches._Shared;

using FluentValidation;

public static class MatchPropertyValidator
{
    public static IRuleBuilderOptions<T, Guid> ValidateTeamId<T>(
        this IRuleBuilder<T, Guid> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("TeamId must not be empty");
}