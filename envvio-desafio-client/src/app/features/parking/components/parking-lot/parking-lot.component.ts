import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { ParkingOperationService } from '../../services/parking-operation.service';
import { ParkingSession } from '../../models/parking-session.model';
import { PaginationParams, PaginatedResult } from '../../../../core/models/pagination.model';
import { EntryDialogComponent } from '../entry-dialog/entry-dialog.component';
import { ExitDialogComponent } from '../exit-dialog/exit-dialog.component';
import { formatLocalTime, calculateDuration, getVehicleTypeDisplay } from '../../../../shared/utils';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-parking-lot',
  standalone: false,
  templateUrl: './parking-lot.component.html',
  styleUrl: './parking-lot.component.scss'
})
export class ParkingLotComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  sessions: ParkingSession[] = [];
  displayedColumns: string[] = ['plate', 'model', 'type', 'entryTime', 'duration', 'actions'];
  isLoading = false;
  errorMessage = '';
  searchQuery = '';
  private searchSubject = new Subject<string>();
  private durationUpdateInterval: any;

  // Pagination
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  pageSizeOptions = [5, 10, 20, 50];

  // Sorting
  sortBy: string = 'entrytime';
  sortOrder: 'asc' | 'desc' = 'asc';

  constructor(
    private parkingService: ParkingOperationService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadSessions();

    // Setup search with debounce
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(query => {
      this.loadSessions(query);
    });

    this.durationUpdateInterval = setInterval(() => {
      this.sessions = [...this.sessions];
    }, 60000);
  }

  ngOnDestroy(): void {
    if (this.durationUpdateInterval) {
      clearInterval(this.durationUpdateInterval);
    }
    this.searchSubject.complete();
  }

  onSearchChange(query: string): void {
    this.searchQuery = query;
    this.searchSubject.next(query);
  }

  loadSessions(plateFilter?: string): void {
    this.isLoading = true;
    this.errorMessage = '';

    const paginationParams: PaginationParams = {
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder
    };

    this.parkingService.getCurrentSessionsPaginated(paginationParams, plateFilter).subscribe({
      next: (response) => {
        const paginatedData = response.data as PaginatedResult<ParkingSession>;
        this.sessions = paginatedData.data || [];
        this.totalCount = paginatedData.totalCount;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Falha ao carregar sessões';
        this.isLoading = false;
        console.error('Error loading sessions:', error);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadSessions(this.searchQuery || undefined);
  }

  onSortChange(sort: Sort): void {
    // Map column name for API
    const columnName = sort.active === 'entryTime' ? 'entrytime' : sort.active;
    
    // Manual toggle logic: if clicking same column, toggle direction
    if (columnName === this.sortBy) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      // New column, start with asc
      this.sortBy = columnName;
      this.sortOrder = 'asc';
    }
    
    this.currentPage = 1;
    this.loadSessions(this.searchQuery || undefined);
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
