import { Routes } from '@angular/router';
import { ROUTE_PATHS } from './app.paths';

export const routes: Routes = [
  {
    path: '',
    redirectTo: `/${ROUTE_PATHS.parking}`,
    pathMatch: 'full'
  },
  {
    path: ROUTE_PATHS.vehicles,
    loadChildren: () => import('./features/vehicles/vehicles.module').then(m => m.VehiclesModule)
  },
  {
    path: ROUTE_PATHS.parking,
    loadChildren: () => import('./features/parking/parking.module').then(m => m.ParkingModule)
  },
  {
    path: ROUTE_PATHS.reports,
    loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule)
  },
  {
    path: ROUTE_PATHS.wildcard,
    redirectTo: `/${ROUTE_PATHS.parking}`,
    pathMatch: 'full'
  }
];
