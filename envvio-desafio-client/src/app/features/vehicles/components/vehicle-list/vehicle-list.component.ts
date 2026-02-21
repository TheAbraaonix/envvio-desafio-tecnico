import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle } from '../../models/vehicle.model';
import { PaginationParams, PaginatedResult } from '../../../../core/models/pagination.model';
import { VehicleFormComponent } from '../vehicle-form/vehicle-form.component';
import { ConfirmationDialogComponent } from '../../../../core/components/confirmation-dialog/confirmation-dialog.component';
import { getVehicleTypeDisplay } from '../../../../shared/utils';

@Component({
  selector: 'app-vehicle-list',
  standalone: false,
  templateUrl: './vehicle-list.component.html',
  styleUrl: './vehicle-list.component.scss'
})
export class VehicleListComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  vehicles: Vehicle[] = [];
  displayedColumns: string[] = ['plate', 'model', 'color', 'type', 'actions'];
  isLoading = false;
  errorMessage = '';

  // Pagination
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  pageSizeOptions = [5, 10, 20, 50];
  
  // Sorting
  sortBy: string = 'plate';
  sortOrder: 'asc' | 'desc' = 'asc';

  // Filter
  plateFilter = '';

  constructor(
    private vehicleService: VehicleService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadVehicles();
  }

  // Expose utility function to template
  protected getVehicleTypeDisplay = getVehicleTypeDisplay;

  loadVehicles(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const paginationParams: PaginationParams = {
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder
    };

    this.vehicleService.getAllPaginated(paginationParams, this.plateFilter || undefined).subscribe({
      next: (response) => {
        const paginatedData = response.data as PaginatedResult<Vehicle>;
        this.vehicles = paginatedData.data || [];
        this.totalCount = paginatedData.totalCount;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar veículos';
        this.isLoading = false;
        console.error('Error loading vehicles:', error);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1; // Material paginator is 0-indexed
    this.pageSize = event.pageSize;
    this.loadVehicles();
  }

  onSortChange(sort: Sort): void {
    // Manual toggle logic: if clicking same column, toggle direction
    if (sort.active === this.sortBy) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      // New column, start with asc
      this.sortBy = sort.active;
      this.sortOrder = 'asc';
    }
    
    this.currentPage = 1;
    this.loadVehicles();
  }

  onFilterChange(): void {
    this.currentPage = 1; // Reset to first page when filter changes
    this.loadVehicles();
  }

  clearFilter(): void {
    this.plateFilter = '';
    this.currentPage = 1;
    this.loadVehicles();
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
