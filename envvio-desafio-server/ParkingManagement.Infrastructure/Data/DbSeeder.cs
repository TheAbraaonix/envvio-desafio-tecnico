using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Only seed if database is empty
        if (context.Vehicles.Any())
            return;

        var vehicles = new List<Vehicle>
        {
            // Cars
            new Vehicle("ABC1234", VehicleType.Car, "Honda Civic", "Prata"),
            new Vehicle("XYZ5678", VehicleType.Car, "Toyota Corolla", "Branco"),
            new Vehicle("DEF9012", VehicleType.Car, "Volkswagen Gol", "Preto"),
            new Vehicle("GHI3456", VehicleType.Car, "Chevrolet Onix", "Vermelho"),
            new Vehicle("JKL7890", VehicleType.Car, "Fiat Argo", "Azul"),
            new Vehicle("MNO1111", VehicleType.Car, "Hyundai HB20", "Cinza"),
            new Vehicle("PQR2222", VehicleType.Car, "Renault Kwid", "Amarelo"),
            new Vehicle("BCD6D66", VehicleType.Car, "Jeep Compass", "Branco"),
            new Vehicle("EFG7E77", VehicleType.Car, "Hyundai Creta", "Prata"),
            new Vehicle("HIJ8888", VehicleType.Car, "Ford Ka", "Branco"),
            new Vehicle("KLM9999", VehicleType.Car, "Nissan Versa", "Vermelho"),
            
            // Motorcycles
            new Vehicle("STU3A33", VehicleType.Motorcycle, "Honda CG 160", "Vermelha"),
            new Vehicle("VWX4B44", VehicleType.Motorcycle, "Yamaha Fazer 250", "Azul"),
            new Vehicle("YZA5C55", VehicleType.Motorcycle, "Honda Biz 125", "Preta")
        };

        await context.Vehicles.AddRangeAsync(vehicles);
        await context.SaveChangesAsync();

        // Get vehicles for creating sessions
        var savedVehicles = context.Vehicles.ToList();
        
        // Create parking sessions with different scenarios
        var now = DateTime.UtcNow;
        var sessions = new List<ParkingSession>();

        // Scenario 1: Currently parked (open sessions)
        // Vehicle parked for 7 hours (will have high fee)
        var longParkedVehicle = savedVehicles.First(v => v.Plate == "ABC1234");
        var longSession = new ParkingSession(longParkedVehicle.Id);
        SetPrivateProperty(longSession, "EntryTime", now.AddHours(-7));
        sessions.Add(longSession);

        // Vehicle parked for 3 hours
        var mediumParkedVehicle = savedVehicles.First(v => v.Plate == "STU3A33");
        var mediumSession = new ParkingSession(mediumParkedVehicle.Id);
        SetPrivateProperty(mediumSession, "EntryTime", now.AddHours(-3));
        sessions.Add(mediumSession);

        // Vehicle parked for 45 minutes
        var shortParkedVehicle = savedVehicles.First(v => v.Plate == "BCD6D66");
        var shortSession = new ParkingSession(shortParkedVehicle.Id);
        SetPrivateProperty(shortSession, "EntryTime", now.AddMinutes(-45));
        sessions.Add(shortSession);

        // Vehicle parked for 10 minutes (within grace period)
        var graceVehicle = savedVehicles.First(v => v.Plate == "MNO1111");
        var graceSession = new ParkingSession(graceVehicle.Id);
        SetPrivateProperty(graceSession, "EntryTime", now.AddMinutes(-10));
        sessions.Add(graceSession);

        // Scenario 2: Historical sessions (closed) for reports
        // Create sessions from last 7 days with varying durations
        for (int day = 1; day <= 7; day++)
        {
            var date = now.AddDays(-day);
            
            // Morning session (9 AM - 11 AM, 2 hours)
            var morningVehicle = savedVehicles[(day - 1) % savedVehicles.Count];
            var morningSession = new ParkingSession(morningVehicle.Id);
            SetPrivateProperty(morningSession, "EntryTime", date.Date.AddHours(9));
            morningSession.RegisterExit(11.00m); // 2 hours = R$11
            SetPrivateProperty(morningSession, "ExitTime", date.Date.AddHours(11));
            sessions.Add(morningSession);

            // Afternoon session (2 PM - 6 PM, 4 hours)
            var afternoonVehicle = savedVehicles[(day * 2) % savedVehicles.Count];
            var afternoonSession = new ParkingSession(afternoonVehicle.Id);
            SetPrivateProperty(afternoonSession, "EntryTime", date.Date.AddHours(14));
            afternoonSession.RegisterExit(17.00m); // 4 hours = R$17
            SetPrivateProperty(afternoonSession, "ExitTime", date.Date.AddHours(18));
            sessions.Add(afternoonSession);

            // Evening session (5 PM - 7 PM, 2 hours)
            var eveningVehicle = savedVehicles[(day * 3) % savedVehicles.Count];
            var eveningSession = new ParkingSession(eveningVehicle.Id);
            SetPrivateProperty(eveningSession, "EntryTime", date.Date.AddHours(17));
            eveningSession.RegisterExit(11.00m); // 2 hours = R$11
            SetPrivateProperty(eveningSession, "ExitTime", date.Date.AddHours(19));
            sessions.Add(eveningSession);
        }

        // Add a few quick stops (within grace period, free)
        for (int day = 1; day <= 3; day++)
        {
            var date = now.AddDays(-day);
            var quickVehicle = savedVehicles[(day + 5) % savedVehicles.Count];
            var quickSession = new ParkingSession(quickVehicle.Id);
            SetPrivateProperty(quickSession, "EntryTime", date.Date.AddHours(12));
            quickSession.RegisterExit(0.00m); // 10 minutes = Free
            SetPrivateProperty(quickSession, "ExitTime", date.Date.AddHours(12).AddMinutes(10));
            sessions.Add(quickSession);
        }

        await context.ParkingSessions.AddRangeAsync(sessions);
        await context.SaveChangesAsync();
    }

    private static void SetPrivateProperty<T>(T obj, string propertyName, object value)
    {
        var property = typeof(T).GetProperty(propertyName, 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
        }
        else
        {
            // Try to use the backing field if property has private setter
            var field = typeof(T).GetField($"<{propertyName}>k__BackingField", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            
            field?.SetValue(obj, value);
        }
    }
}
