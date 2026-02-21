import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { OccupancyByHourDto } from '../../models/report.model';
import { formatLocalHour } from '../../../../shared/utils';

@Component({
  selector: 'app-occupancy-report',
  standalone: false,
  templateUrl: './occupancy-report.component.html',
  styleUrl: './occupancy-report.component.scss'
})
export class OccupancyReportComponent implements OnInit {
  occupancyData: OccupancyByHourDto[] = [];
  displayedColumns: string[] = ['hour', 'vehicleCount'];
  isLoading = false;
  errorMessage = '';
  selectedPeriod = 1; // Default: last 24 hours

  periodOptions = [
    { value: 1, label: 'Últimas 24 horas' },
    { value: 3, label: 'Últimos 3 dias' },
    { value: 7, label: 'Últimos 7 dias' }
  ];

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.loadOccupancy();
  }

  loadOccupancy(): void {
    this.isLoading = true;
    this.errorMessage = '';

    // Calculate date range based on selected period
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - this.selectedPeriod);

    // Format dates as ISO strings for API
    const startDateStr = startDate.toISOString();
    const endDateStr = endDate.toISOString();

    this.reportService.getOccupancyByHour(startDateStr, endDateStr).subscribe({
      next: (response) => {
        this.occupancyData = response.data || [];
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar relatório de ocupação';
        this.isLoading = false;
        console.error('Error loading occupancy report:', error);
      }
    });
  }

  onPeriodChange(): void {
    this.loadOccupancy();
  }

  // Expose utility function to template
  protected formatLocalHour = formatLocalHour;

  getMaxOccupancy(): number {
    if (this.occupancyData.length === 0) return 0;
    return Math.max(...this.occupancyData.map(o => o.vehicleCount));
  }

  getAverageOccupancy(): number {
    if (this.occupancyData.length === 0) return 0;
    const total = this.occupancyData.reduce((sum, o) => sum + o.vehicleCount, 0);
    return Math.round((total / this.occupancyData.length) * 10) / 10; // 1 decimal place
  }
}
