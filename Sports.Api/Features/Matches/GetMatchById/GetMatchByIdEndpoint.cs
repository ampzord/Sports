namespace Sports.Api.Features.Matches.GetMatchById;

using Sports.Api.Features.Matches._Shared.Responses;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class GetMatchByIdEndpoint(IMediator mediator) : Endpoint<GetMatchByIdRequest, MatchResponse>
{

    public override void Configure()
    {
        Get("/api/matches/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<MatchResponse>(200)
            .Produces(404)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Get a match by ID";
        });
    }

    public override async Task HandleAsync(
        GetMatchByIdRequest req,
        CancellationToken ct)
    {
        var query = new GetMatchByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, ct);
    }
}
