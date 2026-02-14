namespace Sports.Api.Features.Matches.UpdateMatch;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Matches._Shared;

public class UpdateMatchEndpoint(IMediator mediator, MatchMapper mapper) : Endpoint<UpdateMatchRequest, MatchResponse>
{

    public override void Configure()
    {
        Put("matches/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<MatchResponse>(200)
            .Produces(400)
            .Produces(404)
            .Produces(409)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Update an existing match";
            s.ExampleRequest = UpdateMatchRequest.Example;
        });
    }

    public override async Task HandleAsync(
        UpdateMatchRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
