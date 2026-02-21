import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ApiResponse } from '../../../core/models/api-response.model';
import { PaginationParams, PaginatedResult } from '../../../core/models/pagination.model';
import { Vehicle, CreateVehicleDto, UpdateVehicleDto } from '../models/vehicle.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly baseUrl = `${environment.apiUrl}${API_ENDPOINTS.vehicles}`;

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<ApiResponse<Vehicle[]>> {
    return this.http.get<ApiResponse<Vehicle[]>>(this.baseUrl);
  }

  getAllPaginated(paginationParams: PaginationParams, plateFilter?: string): Observable<ApiResponse<PaginatedResult<Vehicle>>> {
    let params = new HttpParams()
      .set('page', paginationParams.page.toString())
      .set('pageSize', paginationParams.pageSize.toString());

    if (paginationParams.sortBy) {
      params = params.set('sortBy', paginationParams.sortBy);
    }

    if (paginationParams.sortOrder) {
      params = params.set('sortOrder', paginationParams.sortOrder);
    }

    if (plateFilter) {
      params = params.set('plate', plateFilter);
    }

    return this.http.get<ApiResponse<PaginatedResult<Vehicle>>>(this.baseUrl, { params });
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
