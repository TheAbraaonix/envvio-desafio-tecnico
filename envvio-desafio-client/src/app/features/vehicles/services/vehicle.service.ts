import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ApiResponse } from '../../../core/models/api-response.model';
import { Vehicle, CreateVehicleDto, UpdateVehicleDto } from '../models/vehicle.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly baseUrl = `${environment.apiUrl}${API_ENDPOINTS.vehicles}`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Vehicle[]>> {
    return this.http.get<ApiResponse<Vehicle[]>>(this.baseUrl);
  }

  getById(id: number): Observable<ApiResponse<Vehicle>> {
    return this.http.get<ApiResponse<Vehicle>>(`${this.baseUrl}/${id}`);
  }

  getByPlate(plate: string): Observable<ApiResponse<Vehicle>> {
    return this.http.get<ApiResponse<Vehicle>>(`${this.baseUrl}/plate/${plate}`);
  }

  create(dto: CreateVehicleDto): Observable<ApiResponse<Vehicle>> {
    return this.http.post<ApiResponse<Vehicle>>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateVehicleDto): Observable<ApiResponse<Vehicle>> {
    return this.http.put<ApiResponse<Vehicle>>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.baseUrl}/${id}`);
  }
}
