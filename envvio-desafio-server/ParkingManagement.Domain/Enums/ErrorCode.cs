namespace ParkingManagement.Domain.Enums;

/// <summary>
/// Error codes for standardized error handling and translation
/// </summary>
public enum ErrorCode
{
    // Success (not used for errors, but good for consistency)
    SUCCESS = 0,
    
    // Generic errors (1xxx)
    VALIDATION_ERROR = 1000,
    RESOURCE_NOT_FOUND = 1001,
    CONFLICT = 1002,
    BAD_REQUEST = 1003,
    
    // Vehicle errors (2xxx)
    VEHICLE_NOT_FOUND = 2001,
    VEHICLE_ALREADY_EXISTS = 2002,
    
    // Parking operation errors (3xxx)
    VEHICLE_ALREADY_IN_PARKING = 3001,
    PARKING_SESSION_NOT_FOUND = 3002,
    SESSION_ALREADY_CLOSED = 3003,
    NO_OPEN_SESSION = 3004,
    
    // Internal errors (5xxx)
    INTERNAL_SERVER_ERROR = 5000,
    DATABASE_ERROR = 5001
}
