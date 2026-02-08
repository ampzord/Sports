namespace Sports.Api.Features.Players._Shared;

using FluentValidation;
using Sports.Shared.Entities;

public static class PlayerPropertyValidator
{
    public static IRuleBuilderOptions<T, string> ValidatePlayerName<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    public static IRuleBuilderOptions<T, PlayerPosition> ValidatePlayerPosition<T>(
        this IRuleBuilder<T, PlayerPosition> ruleBuilder) => ruleBuilder
            .IsInEnum().WithMessage("Position must be a valid player position");

    private static bool IsValidPosition(string position)
    {
        if (int.TryParse(position, out _))
            return false;

        return Enum.TryParse<PlayerPosition>(position, ignoreCase: true, out _);
    }
}