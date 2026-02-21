import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { ParkingLotComponent } from './components/parking-lot/parking-lot.component';

const routes: Routes = [
  {
    path: '',
    component: ParkingLotComponent
  }
];

@NgModule({
  declarations: [
    ParkingLotComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class ParkingModule { }
