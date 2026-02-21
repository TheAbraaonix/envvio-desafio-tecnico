export enum VehicleType {
  Car = 'Car',
  Motorcycle = 'Motorcycle'
}

// Translation maps for Vehicle Type
export const VehicleTypeDisplay: { [key in VehicleType]: string } = {
  [VehicleType.Car]: 'Carro',
  [VehicleType.Motorcycle]: 'Moto'
};

export const VehicleTypeFromDisplay: { [key: string]: VehicleType } = {
  'Carro': VehicleType.Car,
  'Moto': VehicleType.Motorcycle
};

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
