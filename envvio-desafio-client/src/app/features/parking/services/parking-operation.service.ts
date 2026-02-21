import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ApiResponse } from '../../../core/models/api-response.model';
import { ParkingSession, RegisterEntryDto, RegisterExitDto, ExitPreviewDto } from '../models/parking-session.model';

@Injectable({
  providedIn: 'root'
})
export class ParkingOperationService {
  private readonly baseUrl = `${environment.apiUrl}${API_ENDPOINTS.parkingOperations}`;

  constructor(private http: HttpClient) { }

  getCurrentSessions(): Observable<ApiResponse<ParkingSession[]>> {
    return this.http.get<ApiResponse<ParkingSession[]>>(`${this.baseUrl}/current`);
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
