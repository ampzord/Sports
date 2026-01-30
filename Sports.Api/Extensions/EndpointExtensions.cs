using FastEndpoints;

namespace Sports.Api.Extensions;

public static class EndpointExtensions
{
    public static void ThrowNotFound<TRequest, TResponse>(
        this Endpoint<TRequest, TResponse> endpoint,
        string message = "Resource not found",
        string errorCode = "NOT_FOUND") where TRequest : notnull
    {
        endpoint.ThrowError(message, errorCode,
            statusCode: StatusCodes.Status404NotFound);
    }
}