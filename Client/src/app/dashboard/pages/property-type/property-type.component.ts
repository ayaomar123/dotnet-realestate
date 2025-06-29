import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { NotificationService } from '../../../shared/services/notification.service';
import { PropertyType } from './interfaces/property-type';
import { PropertyTypeService } from './services/property-type.service';
import { CommonModule } from '@angular/common';
import { ModalComponent } from "../../../shared/components/modal/modal.component";
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";

@Component({
  selector: 'app-property-type',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, ImageUploadComponent],
  templateUrl: './property-type.component.html',
  styleUrl: './property-type.component.css'
})
export class PropertyTypeComponent {
  private readonly service = inject(PropertyTypeService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: PropertyType[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Property Types';

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
    this.service.get().subscribe((res: General<PropertyType[]>) => {
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

  editPropertyType(PropertyType: PropertyType): void {
    this.formMode = 'Edit';
    this.editingId = PropertyType.id;
    this.form.patchValue({
      nameEn: PropertyType.nameEn,
      nameAr: PropertyType.nameAr
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
      this.createPropertyType(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updatePropertyType(this.editingId, formData);
    }
  }

  private createPropertyType(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('PropertyType created successfully!');
      },
      error: err => {
        console.error('Error creating PropertyType:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updatePropertyType(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('PropertyType updated successfully!');
      },
      error: err => {
        console.error('Error updating PropertyType:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(PropertyType: PropertyType): void {
    this.service.updateStatus(PropertyType.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('PropertyType status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deletePropertyType(id: number): void {
    if (confirm('Are you sure you want to delete this PropertyType?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('PropertyType deleted successfully!');
        },
        error: err => {
          console.error('Error deleting PropertyType:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
