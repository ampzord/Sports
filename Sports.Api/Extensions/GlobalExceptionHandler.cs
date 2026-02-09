namespace Sports.Api.Extensions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            Microsoft.Data.SqlClient.SqlException => StatusCodes.Status503ServiceUnavailable,
            RabbitMQ.Client.Exceptions.BrokerUnreachableException => StatusCodes.Status503ServiceUnavailable,
            RabbitMQ.Client.Exceptions.AlreadyClosedException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = exception switch
        {
            Microsoft.Data.SqlClient.SqlException => "Database Unavailable",
            RabbitMQ.Client.Exceptions.BrokerUnreachableException => "Message Broker Unavailable",
            RabbitMQ.Client.Exceptions.AlreadyClosedException => "Message Broker Unavailable",
            _ => "Internal Server Error"
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = title,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Type = httpContext.Response.StatusCode == StatusCodes.Status503ServiceUnavailable
                    ? "https://tools.ietf.org/html/rfc9110#section-15.6.4"
                    : "https://tools.ietf.org/html/rfc9110#section-15.6.1"
            }
        });
    }
}
