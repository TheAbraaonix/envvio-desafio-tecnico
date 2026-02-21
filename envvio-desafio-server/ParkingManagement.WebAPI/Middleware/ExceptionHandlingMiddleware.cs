using System.Net;
using System.Text.Json;
using FluentValidation;
using ParkingManagement.Application.Common;
using ParkingManagement.Domain.Exceptions;

namespace ParkingManagement.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var response = exception switch
        {
            NotFoundException => ApiResponse<object>.ErrorResponse(
                exception.Message,
                (int)HttpStatusCode.NotFound),

            ConflictException => ApiResponse<object>.ErrorResponse(
                exception.Message,
                (int)HttpStatusCode.Conflict),

            BadRequestException => ApiResponse<object>.ErrorResponse(
                exception.Message,
                (int)HttpStatusCode.BadRequest),

            ValidationException validationException => ApiResponse<object>.ErrorResponse(
                "Validation failed",
                (int)HttpStatusCode.BadRequest,
                validationException.Errors.Select(e => e.ErrorMessage).ToList()),

            _ => ApiResponse<object>.ErrorResponse(
                "An internal server error occurred",
                (int)HttpStatusCode.InternalServerError)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
