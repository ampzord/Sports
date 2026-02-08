namespace Sports.Api.Features.Leagues.GetLeagues;

using Sports.Api.Features.Leagues._Shared.Responses;

using FastEndpoints;
using MediatR;

public class GetLeaguesEndpoint(IMediator mediator) : EndpointWithoutRequest<List<LeagueResponse>>
{

    public override void Configure()
    {
        Get("/api/leagues");
        AllowAnonymous();
        Description(b => b
            .Produces<List<LeagueResponse>>(200)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Get all leagues";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetLeaguesQuery();
        var response = await mediator.Send(query, ct);
        await Send.OkAsync(response, ct);
    }
}