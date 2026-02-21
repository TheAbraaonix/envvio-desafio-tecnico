import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';

import { ReportsDashboardComponent } from './components/reports-dashboard/reports-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: ReportsDashboardComponent
  }
];

@NgModule({
  declarations: [
    ReportsDashboardComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatCardModule,
    MatTabsModule
  ]
})
export class ReportsModule { }
