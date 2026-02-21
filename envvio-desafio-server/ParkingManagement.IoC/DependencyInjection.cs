using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Application.Services;
using ParkingManagement.Application.Validators;
using ParkingManagement.Domain.Interfaces;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;

namespace ParkingManagement.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IParkingSessionRepository, ParkingSessionRepository>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateVehicleDtoValidator>();

        // Services
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IParkingOperationService, ParkingOperationService>();
        services.AddScoped<IPricingService, PricingService>();

        return services;
    }
}
