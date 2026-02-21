import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ParkingOperationService } from '../../services/parking-operation.service';
import { ParkingSession } from '../../models/parking-session.model';
import { EntryDialogComponent } from '../entry-dialog/entry-dialog.component';
import { ExitDialogComponent } from '../exit-dialog/exit-dialog.component';
import { formatLocalTime, calculateDuration, getVehicleTypeDisplay } from '../../../../shared/utils';

@Component({
  selector: 'app-parking-lot',
  standalone: false,
  templateUrl: './parking-lot.component.html',
  styleUrl: './parking-lot.component.scss'
})
export class ParkingLotComponent implements OnInit {
  sessions: ParkingSession[] = [];
  displayedColumns: string[] = ['plate', 'model', 'type', 'entryTime', 'duration', 'actions'];
  isLoading = false;
  errorMessage = '';

  constructor(
    private parkingService: ParkingOperationService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadSessions();
  }

  loadSessions(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.parkingService.getCurrentSessions().subscribe({
      next: (response) => {
        this.sessions = response.data || [];
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar sessões';
        this.isLoading = false;
        console.error('Error loading sessions:', error);
      }
    });
  }

  // Expose utility functions to template
  protected formatLocalTime = formatLocalTime;
  protected calculateDuration = calculateDuration;
  protected getVehicleTypeDisplay = getVehicleTypeDisplay;

  openEntryDialog(): void {
    const dialogRef = this.dialog.open(EntryDialogComponent, {
      width: '500px',
      maxHeight: '95vh',
      panelClass: 'entry-dialog-container',
      disableClose: true,
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadSessions();
      }
    });
  }

  openExitDialog(session: ParkingSession): void {
    const dialogRef = this.dialog.open(ExitDialogComponent, {
      width: '500px',
      disableClose: true,
      data: { session }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadSessions();
      }
    });
  }
}
