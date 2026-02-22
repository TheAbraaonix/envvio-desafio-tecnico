using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace ParkingManagement.WebAPI.HealthChecks;

/// <summary>
/// Custom health check response writer that returns detailed JSON instead of plain text.
/// </summary>
public static class HealthCheckResponseWriter
{
    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = new
        {
            status = healthReport.Status.ToString(),
            checks = healthReport.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.TotalMilliseconds,
                exception = entry.Value.Exception?.Message,
                data = entry.Value.Data
            }),
            totalDuration = healthReport.TotalDuration.TotalMilliseconds
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
    }
}
