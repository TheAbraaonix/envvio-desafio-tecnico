import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ApiResponse } from '../../../core/models/api-response.model';
import { PaginationParams, PaginatedResult } from '../../../core/models/pagination.model';
import { ParkingSession, RegisterEntryDto, RegisterExitDto, ExitPreviewDto } from '../models/parking-session.model';

@Injectable({
  providedIn: 'root'
})
export class ParkingOperationService {
  private readonly baseUrl = `${environment.apiUrl}${API_ENDPOINTS.parkingOperations}`;

  constructor(private http: HttpClient) { }

  getCurrentSessions(plateFilter?: string): Observable<ApiResponse<ParkingSession[]>> {
    let params = new HttpParams();
    if (plateFilter) {
      params = params.set('plate', plateFilter);
    }
    return this.http.get<ApiResponse<ParkingSession[]>>(`${this.baseUrl}/open-sessions`, { params });
  }

  getCurrentSessionsPaginated(paginationParams: PaginationParams, plateFilter?: string): Observable<ApiResponse<PaginatedResult<ParkingSession>>> {
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

    return this.http.get<ApiResponse<PaginatedResult<ParkingSession>>>(`${this.baseUrl}/open-sessions`, { params });
  }

  registerEntry(dto: RegisterEntryDto): Observable<ApiResponse<ParkingSession>> {
    return this.http.post<ApiResponse<ParkingSession>>(`${this.baseUrl}/entry`, dto);
  }

  previewExit(plate: string): Observable<ApiResponse<ExitPreviewDto>> {
    return this.http.get<ApiResponse<ExitPreviewDto>>(`${this.baseUrl}/exit-preview/plate/${plate}`);
  }

  registerExit(dto: RegisterExitDto): Observable<ApiResponse<ParkingSession>> {
    return this.http.post<ApiResponse<ParkingSession>>(`${this.baseUrl}/exit`, dto);
  }
}
