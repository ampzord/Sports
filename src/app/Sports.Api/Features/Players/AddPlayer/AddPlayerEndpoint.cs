namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Features.Players._Shared;
using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Players.GetPlayerById;

public class AddPlayerEndpoint(IMediator mediator, PlayerMapper mapper) : Endpoint<AddPlayerRequest, PlayerResponse>
{

    public override void Configure()
    {
        Post("players");
        AllowAnonymous();
        Description(b => b
            .Produces<PlayerResponse>(201)
            .Produces(400)
            .Produces(409)
            .WithTags("Players"));
        Summary(s =>
        {
            s.Summary = "Create a new player";
            s.ExampleRequest = AddPlayerRequest.Example;
        });
    }

    public override async Task HandleAsync(
        AddPlayerRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await this.SendCreatedAtAsync<GetPlayerByIdEndpoint>(result.Value.Id, result.Value, ct);
    }
}
