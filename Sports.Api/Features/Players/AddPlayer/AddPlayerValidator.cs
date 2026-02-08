namespace Sports.Api.Features.Players.AddPlayer;

using FastEndpoints;
using FluentValidation;
using Sports.Api.Features.Players._Shared;

public class AddPlayerValidator : Validator<AddPlayerRequest>
{
    public AddPlayerValidator()
    {
        RuleFor(x => x.Name).ValidatePlayerName();
        //RuleFor(x => x.Position).ValidatePlayerPosition();
    }
}