export enum VehicleType {
  Car = 'Car',
  Motorcycle = 'Motorcycle'
}

export interface Vehicle {
  id: number;
  plate: string;
  model?: string;
  color?: string;
  type: VehicleType;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateVehicleDto {
  plate: string;
  model?: string;
  color?: string;
  type: VehicleType;
}

export interface UpdateVehicleDto {
  model?: string;
  color?: string;
  type: VehicleType;
}
