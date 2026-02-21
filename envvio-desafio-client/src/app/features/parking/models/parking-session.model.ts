import { Vehicle } from './vehicle.model';

export interface ParkingSession {
  id: number;
  vehicleId: number;
  vehicle?: Vehicle;
  entryTime: Date;
  exitTime?: Date;
  amountCharged?: number;
  isOpen: boolean;
  duration?: string;
}

export interface RegisterEntryDto {
  plate: string;
}

export interface RegisterExitDto {
  plate: string;
}

export interface ExitPreviewDto {
  sessionId: number;
  entryTime: Date;
  exitTime: Date;
  duration: string;
  amountCharged: number;
  vehicle?: Vehicle;
}
