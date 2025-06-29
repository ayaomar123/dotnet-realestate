import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { City } from './interfaces/city';
import { CityService } from './services/city.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { CommonModule } from '@angular/common';
import { ModalComponent } from "../../../shared/components/modal/modal.component";
import { General } from '../../../shared/interfaces/General';
import { environment } from '../../../../environments/environment.development';
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";

@Component({
  selector: 'app-city',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, ImageUploadComponent],
  templateUrl: './city.component.html',
  styleUrl: './city.component.css'
})
export class CityComponent {
  private readonly service = inject(CityService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: City[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Cities';

  notifications: { type: string; message: string }[] = [];
  private notificationSubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();
    this.initForm();
    this.subscribeToNotifications();
  }

  private initForm(): void {
    this.form = this.fb.group({
      nameEn: ['', Validators.required],
      nameAr: ['', Validators.required]
    });
  }

  private subscribeToNotifications(): void {
    this.notificationSubscription = this.toastr.notifications$.subscribe(notification => {
      this.notifications.push(notification);
      setTimeout(() => this.notifications.shift(), 3000);
    });
  }

  private loadData(): void {
    this.service.get().subscribe((res: General<City[]>) => {
      this.data = res.data;
    });
  }

  getImageUrl(image?: string | null): string {
    return image ? `${environment.assetsUrl}/${image}` : '';
  }

  openModal(): void {
    this.formMode = 'Create';
    this.editingId = null;
    this.selectedImageFile = undefined;
    this.form.reset();
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  editCity(City: City): void {
    this.formMode = 'Edit';
    this.editingId = City.id;
    this.form.patchValue({
      nameEn: City.nameEn,
      nameAr: City.nameAr
    });
    this.isModalOpen = true;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const formData = new FormData();
    formData.append('NameEn', formValue.nameEn || '');
    formData.append('NameAr', formValue.nameAr || '');

    if (this.selectedImageFile) {
      formData.append('Image', this.selectedImageFile);
    }

    if (this.formMode === 'Create') {
      this.createCity(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updateCity(this.editingId, formData);
    }
  }

  private createCity(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('City created successfully!');
      },
      error: err => {
        console.error('Error creating City:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updateCity(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('City updated successfully!');
      },
      error: err => {
        console.error('Error updating City:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(City: City): void {
    this.service.updateStatus(City.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('City status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteCity(id: number): void {
    if (confirm('Are you sure you want to delete this City?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('City deleted successfully!');
        },
        error: err => {
          console.error('Error deleting City:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
