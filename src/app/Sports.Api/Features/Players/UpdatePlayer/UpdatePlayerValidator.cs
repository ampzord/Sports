namespace Sports.Api.Features.Players.UpdatePlayer;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Players._Shared;

public class UpdatePlayerValidator : Validator<UpdatePlayerRequest>
{
    public UpdatePlayerValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.Name).ValidatePlayerName();
        //RuleFor(x => x.Position).ValidatePlayerPosition();
        RuleFor(x => x.TeamId)
            .GreaterThan(0).When(x => x.TeamId.HasValue)
            .WithMessage("TeamId must be greater than 0");
    }
}