namespace Sports.Api.Features.Matches.SimulateMatches;

using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;

public class SimulateMatchesEndpoint(IMediator mediator)
    : EndpointWithoutRequest<SimulateMatchesResponse>
{
    public override void Configure()
    {
        Post("/api/matches/simulate");
        AllowAnonymous();
        Description(x => x
            .Produces<SimulateMatchesResponse>(200)
            .WithTags("Matches"));
        Summary(s =>
        {
            s.Summary = "Trigger all matches simulation";
        });
        
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var command = new SimulateMatchesCommand();
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await Send.OkAsync(result.Value, cancellation: ct);
    }
}
