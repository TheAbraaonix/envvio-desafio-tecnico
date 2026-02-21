/**
 * Error codes for standardized error handling and translation.
 * Mirrors backend ErrorCode enum for consistency.
 * Uses string values to match backend JSON serialization.
 */
export enum ErrorCode {
  // Success
  SUCCESS = 'SUCCESS',
  
  // Generic errors (1xxx)
  VALIDATION_ERROR = 'VALIDATION_ERROR',
  RESOURCE_NOT_FOUND = 'RESOURCE_NOT_FOUND',
  CONFLICT = 'CONFLICT',
  BAD_REQUEST = 'BAD_REQUEST',
  
  // Vehicle errors (2xxx)
  VEHICLE_NOT_FOUND = 'VEHICLE_NOT_FOUND',
  VEHICLE_ALREADY_EXISTS = 'VEHICLE_ALREADY_EXISTS',
  
  // Parking operation errors (3xxx)
  VEHICLE_ALREADY_IN_PARKING = 'VEHICLE_ALREADY_IN_PARKING',
  PARKING_SESSION_NOT_FOUND = 'PARKING_SESSION_NOT_FOUND',
  SESSION_ALREADY_CLOSED = 'SESSION_ALREADY_CLOSED',
  NO_OPEN_SESSION = 'NO_OPEN_SESSION',
  
  // Internal errors (5xxx)
  INTERNAL_SERVER_ERROR = 'INTERNAL_SERVER_ERROR',
  DATABASE_ERROR = 'DATABASE_ERROR'
}
