import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

// Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

// Components
import { ReportsDashboardComponent } from './components/reports-dashboard/reports-dashboard.component';
import { RevenueReportComponent } from './components/revenue-report/revenue-report.component';
import { TopVehiclesReportComponent } from './components/top-vehicles-report/top-vehicles-report.component';
import { OccupancyReportComponent } from './components/occupancy-report/occupancy-report.component';

const routes: Routes = [
  {
    path: '',
    component: ReportsDashboardComponent
  }
];

@NgModule({
  declarations: [
    ReportsDashboardComponent,
    RevenueReportComponent,
    TopVehiclesReportComponent,
    OccupancyReportComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    MatCardModule,
    MatTableModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule
  ]
})
export class ReportsModule { }

