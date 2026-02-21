import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ParkingOperationService } from '../../services/parking-operation.service';
import { ParkingSession, ExitPreviewDto, RegisterExitDto } from '../../models/parking-session.model';
import { formatLocalTime, formatDuration, formatCurrency, getVehicleTypeDisplay } from '../../../../shared/utils';

@Component({
  selector: 'app-exit-dialog',
  standalone: false,
  templateUrl: './exit-dialog.component.html',
  styleUrl: './exit-dialog.component.scss'
})
export class ExitDialogComponent implements OnInit {
  isLoadingPreview = true;
  isSubmitting = false;
  preview: ExitPreviewDto | null = null;
  errorMessage = '';

  constructor(
    private parkingService: ParkingOperationService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ExitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { session: ParkingSession }
  ) {}

  ngOnInit(): void {
    this.loadPreview();
  }

  loadPreview(): void {
    const plate = this.data.session.vehicle?.plate;
    if (!plate) {
      this.errorMessage = 'Placa do veículo não encontrada';
      this.isLoadingPreview = false;
      return;
    }

    this.parkingService.previewExit(plate).subscribe({
      next: (response) => {
        this.preview = response.data;
        this.isLoadingPreview = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar prévia de saída';
        this.isLoadingPreview = false;
        console.error('Error loading preview:', error);
      }
    });
  }

  // Expose utility functions to template
  protected formatLocalTime = formatLocalTime;
  protected formatDuration = formatDuration;
  protected formatCurrency = formatCurrency;
  protected getVehicleTypeDisplay = getVehicleTypeDisplay;

  onConfirm(): void {
    if (!this.preview) return;

    this.isSubmitting = true;

    const dto: RegisterExitDto = {
      plate: this.data.session.vehicle?.plate || ''
    };

    this.parkingService.registerExit(dto).subscribe({
      next: (response) => {
        this.snackBar.open('Saída registrada com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.snackBar.open(error.message || 'Falha ao registrar saída', 'Fechar', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
