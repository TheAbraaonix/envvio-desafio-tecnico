import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle, VehicleTypeDisplay } from '../../models/vehicle.model';
import { VehicleFormComponent } from '../vehicle-form/vehicle-form.component';
import { ConfirmationDialogComponent } from '../../../../core/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-vehicle-list',
  standalone: false,
  templateUrl: './vehicle-list.component.html',
  styleUrl: './vehicle-list.component.scss'
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[] = [];
  displayedColumns: string[] = ['plate', 'model', 'color', 'type', 'actions'];
  isLoading = false;
  errorMessage = '';

  constructor(
    private vehicleService: VehicleService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadVehicles();
  }

  getVehicleTypeDisplay(type: any): string {
    return VehicleTypeDisplay[type as keyof typeof VehicleTypeDisplay] || type;
  }

  loadVehicles(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.vehicleService.getAll().subscribe({
      next: (response) => {
        this.vehicles = response.data || [];
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar veículos';
        this.isLoading = false;
        console.error('Error loading vehicles:', error);
      }
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(VehicleFormComponent, {
      width: '500px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadVehicles();
      }
    });
  }

  openEditDialog(vehicle: Vehicle): void {
    const dialogRef = this.dialog.open(VehicleFormComponent, {
      width: '500px',
      disableClose: true,
      data: { vehicle }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadVehicles();
      }
    });
  }

  deleteVehicle(id: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Excluir Veículo',
        message: 'Tem certeza que deseja excluir este veículo? Esta ação não pode ser desfeita.',
        confirmText: 'Excluir',
        cancelText: 'Cancelar',
        confirmColor: 'warn'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.vehicleService.delete(id).subscribe({
          next: () => {
            this.loadVehicles();
          },
          error: (error) => {
            this.errorMessage = error.message || 'Falha ao excluir veículo';
            console.error('Error deleting vehicle:', error);
          }
        });
      }
    });
  }
}
