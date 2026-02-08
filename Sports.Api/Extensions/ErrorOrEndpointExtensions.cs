namespace Sports.Api.Extensions;

using ErrorOr;
using FastEndpoints;

public static class ErrorOrEndpointExtensions
{
    public static async Task SendErrorAsync(
        this IEndpoint endpoint,
        Error error,
        CancellationToken ct)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status400BadRequest
        };

        await endpoint.HttpContext.Response.SendAsync(
            new { error.Code, error.Description },
            statusCode,
            cancellation: ct);
    }
}
