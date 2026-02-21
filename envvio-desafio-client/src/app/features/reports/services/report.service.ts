import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ApiResponse } from '../../../core/models/api-response.model';
import { RevenueByDayDto, VehicleParkingTimeDto, OccupancyByHourDto } from '../models/report.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private readonly baseUrl = `${environment.apiUrl}${API_ENDPOINTS.reports}`;

  constructor(private http: HttpClient) { }

  getRevenueByDay(days: number = 7): Observable<ApiResponse<RevenueByDayDto[]>> {
    const params = new HttpParams().set('days', days.toString());
    return this.http.get<ApiResponse<RevenueByDayDto[]>>(`${this.baseUrl}/revenue-by-day`, { params });
  }

  getTopVehiclesByParkingTime(
    startDate?: string,
    endDate?: string,
    top: number = 10
  ): Observable<ApiResponse<VehicleParkingTimeDto[]>> {
    let params = new HttpParams().set('top', top.toString());
    
    if (startDate) {
      params = params.set('startDate', startDate);
    }
    if (endDate) {
      params = params.set('endDate', endDate);
    }

    return this.http.get<ApiResponse<VehicleParkingTimeDto[]>>(
      `${this.baseUrl}/top-vehicles-by-parking-time`,
      { params }
    );
  }

  getOccupancyByHour(startDate: string, endDate: string): Observable<ApiResponse<OccupancyByHourDto[]>> {
    const params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);

    return this.http.get<ApiResponse<OccupancyByHourDto[]>>(
      `${this.baseUrl}/occupancy-by-hour`,
      { params }
    );
  }
}
