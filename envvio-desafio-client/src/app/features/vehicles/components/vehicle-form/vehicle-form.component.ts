import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle, CreateVehicleDto, UpdateVehicleDto, VehicleType, VehicleTypeDisplay, VehicleTypeFromDisplay } from '../../models/vehicle.model';

@Component({
  selector: 'app-vehicle-form',
  standalone: false,
  templateUrl: './vehicle-form.component.html',
  styleUrl: './vehicle-form.component.scss'
})
export class VehicleFormComponent implements OnInit {
  vehicleForm: FormGroup;
  isEditMode = false;
  isSubmitting = false;
  vehicleTypes = ['Carro', 'Moto'];

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<VehicleFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { vehicle?: Vehicle }
  ) {
    this.vehicleForm = this.fb.group({
      plate: ['', [
        Validators.required,
        Validators.minLength(7),
        Validators.maxLength(7),
        // Brazilian plate patterns: LLLNNNN (old) or LLLNLNN (Mercosul)
        Validators.pattern(/^[A-Z]{3}([0-9]{4}|[0-9][A-J][0-9]{2})$/)
      ]],
      model: ['', [
        Validators.required,
        Validators.maxLength(100)
      ]],
      color: ['', Validators.maxLength(50)],
      type: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.data?.vehicle) {
      this.isEditMode = true;
      this.vehicleForm.patchValue({
        plate: this.data.vehicle.plate,
        model: this.data.vehicle.model,
        color: this.data.vehicle.color,
        type: VehicleTypeDisplay[this.data.vehicle.type] // Convert API value to display value
      });
      // Disable plate editing in edit mode (plates shouldn't change)
      this.vehicleForm.get('plate')?.disable();
    }
  }

  get plate() {
    return this.vehicleForm.get('plate');
  }

  get model() {
    return this.vehicleForm.get('model');
  }

  get color() {
    return this.vehicleForm.get('color');
  }

  get type() {
    return this.vehicleForm.get('type');
  }

  onSubmit(): void {
    if (this.vehicleForm.invalid) {
      this.vehicleForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    if (this.isEditMode) {
      this.updateVehicle();
    } else {
      this.createVehicle();
    }
  }

  private createVehicle(): void {
    const displayType = this.vehicleForm.value.type;
    const apiType = VehicleTypeFromDisplay[displayType]; // Convert display value to API value
    
    const dto: CreateVehicleDto = {
      plate: this.vehicleForm.value.plate.toUpperCase(),
      model: this.vehicleForm.value.model,
      color: this.vehicleForm.value.color || undefined,
      type: apiType
    };

    this.vehicleService.create(dto).subscribe({
      next: (response) => {
        this.snackBar.open('Veículo criado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.dialogRef.close(true);
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

  private updateVehicle(): void {
    const displayType = this.vehicleForm.value.type;
    const apiType = VehicleTypeFromDisplay[displayType]; // Convert display value to API value
    
    const dto: UpdateVehicleDto = {
      model: this.vehicleForm.value.model,
      color: this.vehicleForm.value.color || undefined,
      type: apiType
    };

    this.vehicleService.update(this.data.vehicle!.id, dto).subscribe({
      next: (response) => {
        this.snackBar.open('Veículo atualizado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.snackBar.open(error.message || 'Falha ao atualizar veículo', 'Fechar', {
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
