using System.Net;
using System.Text.Json;
using FluentValidation;
using ParkingManagement.Application.Common;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Domain.Exceptions.Base;
using ParkingManagement.Domain.Exceptions.Vehicle;
using ParkingManagement.Domain.Exceptions.ParkingOperation;

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

        var (statusCode, errorCode, message, errors) = exception switch
        {
            // Specific vehicle exceptions (most specific first)
            VehicleAlreadyExistsException => (
                HttpStatusCode.Conflict,
                ErrorCode.VEHICLE_ALREADY_EXISTS,
                exception.Message,
                null as List<string>),

            VehicleNotFoundException => (
                HttpStatusCode.NotFound,
                ErrorCode.VEHICLE_NOT_FOUND,
                exception.Message,
                null as List<string>),

            // Specific parking operation exceptions
            VehicleAlreadyInParkingException => (
                HttpStatusCode.Conflict,
                ErrorCode.VEHICLE_ALREADY_IN_PARKING,
                exception.Message,
                null as List<string>),

            ParkingSessionNotFoundException => (
                HttpStatusCode.NotFound,
                ErrorCode.PARKING_SESSION_NOT_FOUND,
                exception.Message,
                null as List<string>),

            SessionAlreadyClosedException => (
                HttpStatusCode.BadRequest,
                ErrorCode.SESSION_ALREADY_CLOSED,
                exception.Message,
                null as List<string>),

            // FluentValidation exception
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                ErrorCode.VALIDATION_ERROR,
                "Validation failed",
                validationException.Errors.Select(e => e.ErrorMessage).ToList()),

            // Generic parking operation exception (catch-all)
            InvalidParkingOperationException => (
                HttpStatusCode.BadRequest,
                ErrorCode.BAD_REQUEST,
                exception.Message,
                null as List<string>),

            // Base exceptions (fallback for any unmapped specific exceptions)
            NotFoundException => (
                HttpStatusCode.NotFound,
                ErrorCode.RESOURCE_NOT_FOUND,
                exception.Message,
                null as List<string>),

            ConflictException => (
                HttpStatusCode.Conflict,
                ErrorCode.CONFLICT,
                exception.Message,
                null as List<string>),

            BadRequestException => (
                HttpStatusCode.BadRequest,
                ErrorCode.BAD_REQUEST,
                exception.Message,
                null as List<string>),

            // Completely unknown exception (safety net)
            _ => (
                HttpStatusCode.InternalServerError,
                ErrorCode.INTERNAL_SERVER_ERROR,
                "An internal server error occurred",
                null as List<string>)
        };

        var response = ApiResponse<object>.ErrorResponse(
            message,
            errorCode,
            (int)statusCode,
            errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
