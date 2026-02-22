using ParkingManagement.IoC;
using ParkingManagement.WebAPI.Middleware;
using ParkingManagement.WebAPI.HealthChecks;
using ParkingManagement.Infrastructure.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()  // IMPORTANT: This enables correlation ID to appear in logs
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.log",  // File pattern: logs/app-2026-02-22.log
        rollingInterval: RollingInterval.Day,  // New file each day
        retainedFileCountLimit: 30,  // Keep last 30 days
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Parking Management API...");

    var builder = WebApplication.CreateBuilder(args);

    // Replace default logging with Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Convert enums to strings in JSON responses
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add health checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>("database");  // Checks if database is accessible

    // Configure CORS for Angular frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    // Apply migrations and seed database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
        // Ensure database is created and migrations applied
        await context.Database.EnsureCreatedAsync();
    
        // Seed demo data
        await DbSeeder.SeedAsync(context);
    }

    // This ensures every log (including from other middleware) has the correlation ID
    app.UseMiddleware<CorrelationIdMiddleware>();

    // Global exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Serilog request logging - logs HTTP requests automatically
    // This will log: Method, Path, StatusCode, Duration
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Removed UseHttpsRedirection for local development with HTTP
    app.UseCors("AllowAngularApp");

    app.UseAuthorization();

    app.MapControllers();

    // Map health check endpoint
    // Available at: GET /health
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteResponse
    });

    Log.Information("Parking Management API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.Information("Shutting down Parking Management API...");
    Log.CloseAndFlush();  // Ensure all logs are written before exit
}
