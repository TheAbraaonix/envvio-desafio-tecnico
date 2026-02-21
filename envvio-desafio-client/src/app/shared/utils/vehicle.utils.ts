import { VehicleTypeDisplay } from '../../features/vehicles/models/vehicle.model';

/**
 * Gets the display name for a vehicle type
 * @param type - Vehicle type enum value
 * @returns Portuguese display name (e.g., "Carro", "Moto")
 */
export function getVehicleTypeDisplay(type: any): string {
  return VehicleTypeDisplay[type as keyof typeof VehicleTypeDisplay] || type;
}
