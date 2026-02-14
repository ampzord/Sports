namespace Sports.Api.Features.Leagues.AddLeague;

using Sports.Api.Features.Leagues._Shared;
using FastEndpoints;
using MediatR;
using Sports.Api.Extensions;
using Sports.Api.Features.Leagues.GetLeagueById;

public class AddLeagueEndpoint(IMediator mediator, LeagueMapper mapper) : Endpoint<AddLeagueRequest, LeagueResponse>
{

    public override void Configure()
    {
        Post("leagues");
        AllowAnonymous();
        Description(b => b
            .Produces<LeagueResponse>(201)
            .Produces(400)
            .Produces(409)
            .WithTags("Leagues"));
        Summary(s =>
        {
            s.Summary = "Create a new league";
            s.ExampleRequest = AddLeagueRequest.Example;
        });
    }

    public override async Task HandleAsync(
        AddLeagueRequest req,
        CancellationToken ct)
    {
        var command = mapper.ToCommand(req);
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            await this.SendErrorsAsync(result.Errors, ct);
            return;
        }

        await this.SendCreatedAtAsync<GetLeagueByIdEndpoint>(
            result.Value.Id, result.Value, ct);
    }
}
