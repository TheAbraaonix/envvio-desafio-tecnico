import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { RevenueByDayDto } from '../../models/report.model';
import { formatCurrency } from '../../../../shared/utils';

@Component({
  selector: 'app-revenue-report',
  standalone: false,
  templateUrl: './revenue-report.component.html',
  styleUrl: './revenue-report.component.scss'
})
export class RevenueReportComponent implements OnInit {
  revenueData: RevenueByDayDto[] = [];
  displayedColumns: string[] = ['date', 'sessionCount', 'totalRevenue'];
  isLoading = false;
  errorMessage = '';
  selectedPeriod = 7; // Default: last 7 days
  
  periodOptions = [
    { value: 7, label: 'Últimos 7 dias' },
    { value: 15, label: 'Últimos 15 dias' },
    { value: 30, label: 'Últimos 30 dias' }
  ];

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.loadRevenue();
  }

  loadRevenue(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.reportService.getRevenueByDay(this.selectedPeriod).subscribe({
      next: (response) => {
        this.revenueData = response.data || [];
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar relatório de receita';
        this.isLoading = false;
        console.error('Error loading revenue report:', error);
      }
    });
  }

  onPeriodChange(): void {
    this.loadRevenue();
  }

  formatCurrency = formatCurrency;

  formatDate(date: Date | string): string {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${day}/${month}/${year}`;
  }

  getTotalRevenue(): number {
    return this.revenueData.reduce((sum, item) => sum + item.totalRevenue, 0);
  }

  getTotalSessions(): number {
    return this.revenueData.reduce((sum, item) => sum + item.sessionCount, 0);
  }
}
