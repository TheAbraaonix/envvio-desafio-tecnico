import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ParkingOperationService } from '../../services/parking-operation.service';
import { VehicleService } from '../../../vehicles/services/vehicle.service';
import { RegisterEntryDto } from '../../models/parking-session.model';
import { CreateVehicleDto } from '../../../vehicles/models/vehicle.model';

@Component({
  selector: 'app-entry-dialog',
  standalone: false,
  templateUrl: './entry-dialog.component.html',
  styleUrl: './entry-dialog.component.scss'
})
export class EntryDialogComponent implements OnInit {
  entryForm: FormGroup;
  isSubmitting = false;
  isCheckingPlate = false;
  showVehicleForm = false;
  vehicleTypes = ['Carro', 'Moto'];
  vehicleTypeMap: { [key: string]: string } = {
    'Carro': 'Car',
    'Moto': 'Motorcycle'
  };

  constructor(
    private fb: FormBuilder,
    private parkingService: ParkingOperationService,
    private vehicleService: VehicleService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<EntryDialogComponent>
  ) {
    this.entryForm = this.fb.group({
      plate: ['', [
        Validators.required,
        Validators.minLength(7),
        Validators.maxLength(7),
        Validators.pattern(/^[A-Z0-9]{7}$/)
      ]],
      model: [''],
      color: [''],
      type: ['']
    });
  }

  ngOnInit(): void {}

  get plate() {
    return this.entryForm.get('plate');
  }

  get model() {
    return this.entryForm.get('model');
  }

  get color() {
    return this.entryForm.get('color');
  }

  get type() {
    return this.entryForm.get('type');
  }

  onPlateBlur(): void {
    const plateValue = this.plate?.value;
    
    // Reset vehicle form state
    this.showVehicleForm = false;
    this.model?.clearValidators();
    this.type?.clearValidators();
    this.color?.clearValidators();
    this.model?.updateValueAndValidity();
    this.type?.updateValueAndValidity();
    this.color?.updateValueAndValidity();

    if (plateValue && plateValue.length === 7 && this.plate?.valid) {
      this.checkPlateExists(plateValue.toUpperCase());
    }
  }

  checkPlateExists(plate: string): void {
    this.isCheckingPlate = true;

    this.vehicleService.getByPlate(plate).subscribe({
      next: (response) => {
        // Vehicle exists, don't show form
        this.showVehicleForm = false;
        this.isCheckingPlate = false;
      },
      error: (error) => {
        // Vehicle not found, show creation form
        this.showVehicleForm = true;
        this.isCheckingPlate = false;
        
        // Add validators to vehicle fields
        this.model?.setValidators([Validators.required, Validators.maxLength(100)]);
        this.color?.setValidators([Validators.maxLength(50)]);
        this.type?.setValidators([Validators.required]);
        this.model?.updateValueAndValidity();
        this.color?.updateValueAndValidity();
        this.type?.updateValueAndValidity();
      }
    });
  }

  onSubmit(): void {
    if (this.entryForm.invalid) {
      this.entryForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    // If vehicle form is shown, create vehicle first
    if (this.showVehicleForm) {
      this.createVehicleAndRegisterEntry();
    } else {
      this.registerEntry();
    }
  }

  createVehicleAndRegisterEntry(): void {
    const displayType = this.entryForm.value.type;
    const apiType = this.vehicleTypeMap[displayType];

    const vehicleDto: CreateVehicleDto = {
      plate: this.entryForm.value.plate.toUpperCase(),
      model: this.entryForm.value.model,
      color: this.entryForm.value.color || undefined,
      type: apiType as any
    };

    this.vehicleService.create(vehicleDto).subscribe({
      next: (response) => {
        // Vehicle created, now register entry
        this.registerEntry();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.snackBar.open(error.message || 'Falha ao criar veículo', 'Fechar', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  registerEntry(): void {
    const dto: RegisterEntryDto = {
      plate: this.entryForm.value.plate.toUpperCase()
    };

    this.parkingService.registerEntry(dto).subscribe({
      next: (response) => {
        const message = this.showVehicleForm 
          ? 'Veículo criado e entrada registrada com sucesso!' 
          : 'Entrada registrada com sucesso!';
        
        this.snackBar.open(message, 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.snackBar.open(error.message || 'Falha ao registrar entrada', 'Fechar', {
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
