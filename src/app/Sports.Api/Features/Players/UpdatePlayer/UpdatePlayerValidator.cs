namespace Sports.Api.Features.Players.UpdatePlayer;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Players._Shared;

public class UpdatePlayerValidator : Validator<UpdatePlayerRequest>
{
    public UpdatePlayerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(x => x.Name).ValidatePlayerName();
        //RuleFor(x => x.Position).ValidatePlayerPosition();
        RuleFor(x => x.TeamId)
            .NotEmpty().When(x => x.TeamId.HasValue)
            .WithMessage("TeamId must not be empty");
    }
}