export interface RevenueByDayDto {
  date: Date;
  totalRevenue: number;
  sessionCount: number;
}

export interface VehicleParkingTimeDto {
  vehicleId: number;
  plate: string;
  model?: string;
  totalParkingTime: string;
  sessionCount: number;
}

export interface OccupancyByHourDto {
  hour: Date;
  vehicleCount: number;
}
