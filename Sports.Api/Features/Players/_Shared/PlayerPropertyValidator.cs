namespace Sports.Api.Features.Players._Shared;

using FluentValidation;
using Sports.Api.Entities;

public static class PlayerPropertyValidator
{
    public static IRuleBuilderOptions<T, string> ValidatePlayerName<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    public static IRuleBuilderOptions<T, string> ValidatePlayerPosition<T>(
        this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
            .NotEmpty().WithMessage("Position is required")
            .Must(IsValidPosition)
            .WithMessage("Position must be one of: GK, CB, LB, RB, LWB, RWB, CDM, CM, CAM, LM, RM, LW, RW, ST, CF");

    private static bool IsValidPosition(string position)
    {
        if (int.TryParse(position, out _))
            return false;

        return Enum.TryParse<PlayerPosition>(position, ignoreCase: true, out _);
    }
}