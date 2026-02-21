import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { VehicleParkingTimeDto } from '../../models/report.model';
import { formatDuration } from '../../../../shared/utils';

@Component({
  selector: 'app-top-vehicles-report',
  standalone: false,
  templateUrl: './top-vehicles-report.component.html',
  styleUrl: './top-vehicles-report.component.scss'
})
export class TopVehiclesReportComponent implements OnInit {
  vehiclesData: VehicleParkingTimeDto[] = [];
  displayedColumns: string[] = ['rank', 'plate', 'model', 'sessionCount', 'totalParkingTime'];
  isLoading = false;
  errorMessage = '';
  topCount = 10;
  selectedPeriod = 30; // Default: last 30 days

  topOptions = [
    { value: 5, label: 'Top 5' },
    { value: 10, label: 'Top 10' },
    { value: 20, label: 'Top 20' }
  ];

  periodOptions = [
    { value: 7, label: 'Últimos 7 dias' },
    { value: 15, label: 'Últimos 15 dias' },
    { value: 30, label: 'Últimos 30 dias' },
    { value: 60, label: 'Últimos 60 dias' }
  ];

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.loadTopVehicles();
  }

  loadTopVehicles(): void {
    this.isLoading = true;
    this.errorMessage = '';

    // Calculate date range based on selected period
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - this.selectedPeriod);

    // Format dates as ISO strings for API
    const startDateStr = startDate.toISOString();
    const endDateStr = endDate.toISOString();

    this.reportService.getTopVehiclesByParkingTime(startDateStr, endDateStr, this.topCount).subscribe({
      next: (response) => {
        this.vehiclesData = response.data || [];
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar relatório de veículos';
        this.isLoading = false;
        console.error('Error loading top vehicles report:', error);
      }
    });
  }

  onTopCountChange(): void {
    this.loadTopVehicles();
  }

  onPeriodChange(): void {
    this.loadTopVehicles();
  }

  formatDuration = formatDuration;
}
