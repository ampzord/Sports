namespace Sports.Shared.Behaviors;

using MediatR;
using Serilog;

public class ExceptionHandlingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception for request {RequestType}", typeof(TRequest).Name);
            throw;
        }
    }
}
