using Serilog.Context;

namespace ParkingManagement.WebAPI.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request already has a correlation ID (from upstream services)
        // If not, generate a new one
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
                            ?? Guid.NewGuid().ToString();

        // Add the correlation ID to the response headers so clients can reference it
        context.Response.Headers.Append(CorrelationIdHeader, correlationId);

        // Push the correlation ID into Serilog's LogContext
        // This makes it available to all log entries within this request scope
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Continue processing the request
            await _next(context);
        }
    }
}
