namespace Sports.Api.Extensions;

using System.Diagnostics;
using ErrorOr;
using FastEndpoints;

public static class ErrorOrEndpointExtensions
{
    public static async Task SendErrorsAsync(
        this IEndpoint endpoint,
        IReadOnlyList<Error> errors,
        CancellationToken ct)
    {
        var primaryError = errors[0];

        var statusCode = primaryError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status400BadRequest
        };

        var httpContext = endpoint.HttpContext;

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = statusCode,
            Title = primaryError.Code,
            Detail = primaryError.Description,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            Type = primaryError.Type switch
            {
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                ErrorType.Validation => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                _ => "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            },
            Extensions =
            {
                { "traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier }
            }
        };

        if (errors.Count > 1)
        {
            problemDetails.Extensions["errors"] = errors.Select(e => new
            {
                name = e.Code,
                reason = e.Description,
                code = e.Code,
                severity = "Error"
            }).ToArray();
        }

        await httpContext.Response.SendAsync(problemDetails, statusCode, cancellation: ct);
    }
}
