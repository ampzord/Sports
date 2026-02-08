namespace Sports.Api.Features.Matches.AddMatch;

using Sports.Api.Features.Matches._Shared;
using Sports.Api.Features.Matches._Shared.Responses;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Matches.GetMatchById;

public class AddMatchEndpoint(IMediator mediator, MatchMapper mapper) : Endpoint<AddMatchRequest, MatchResponse>
{

    public override void Configure()
    {
        Post("/api/matches");
        AllowAnonymous();
        Description(b => b
            .Produces<MatchResponse>(201)
            .Produces(400)
            .Produces(409)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Create a new match";
            s.ExampleRequest = AddMatchRequest.Example;
        });
    }

    public override async Task HandleAsync(
        AddMatchRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorAsync(result.FirstError, ct);
            return;
        }

        await this.SendCreatedAtAsync<GetMatchByIdEndpoint>(
            result.Value.Id, result.Value, ct);
    }
}