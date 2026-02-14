namespace Sports.Api.Extensions;

using FastEndpoints;

public static class EndpointExtensions
{
    public static async Task SendCreatedAtAsync<TGetEndpoint>(
        this IEndpoint endpoint,
        int id,
        object response,
        CancellationToken ct)
        where TGetEndpoint : IEndpoint
    {
        await endpoint.HttpContext.Response.SendCreatedAtAsync<TGetEndpoint>(
            new { id },
            response,
            cancellation: ct);
    }
}
