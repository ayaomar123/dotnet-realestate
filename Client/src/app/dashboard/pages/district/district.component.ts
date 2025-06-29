import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { NotificationService } from '../../../shared/services/notification.service';
import { DistrictService } from './services/district.service';
import { District } from './interfaces/district';
import { CommonModule } from '@angular/common';
import { ModalComponent } from "../../../shared/components/modal/modal.component";
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";
import { City } from '../city/interfaces/city';
import { CityService } from '../city/services/city.service';

@Component({
  selector: 'app-district',
  imports: [CommonModule, ModalComponent, ReactiveFormsModule, ImageUploadComponent],
  templateUrl: './district.component.html',
  styleUrl: './district.component.css'
})
export class DistrictComponent {
  private readonly service = inject(DistrictService);
  private readonly cityService = inject(CityService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: District[] = [];
  cities: City[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Districts';

  notifications: { type: string; message: string }[] = [];
  private notificationSubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();
    this.loadCities();
    this.initForm();
    this.subscribeToNotifications();
  }

  private initForm(): void {
    this.form = this.fb.group({
      nameEn: ['', Validators.required],
      nameAr: ['', Validators.required],
      cityId: [null, Validators.required]
    });
  }

  private subscribeToNotifications(): void {
    this.notificationSubscription = this.toastr.notifications$.subscribe(notification => {
      this.notifications.push(notification);
      setTimeout(() => this.notifications.shift(), 3000);
    });
  }

  private loadData(): void {
    this.service.get().subscribe((res: General<District[]>) => {
      this.data = res.data;
    });
  }

  private loadCities(): void {
    this.cityService.get().subscribe((res: General<City[]>) => {
      this.cities = res.data;
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

  editDistrict(District: District): void {
    this.formMode = 'Edit';
    this.editingId = District.id;
    this.form.patchValue({
      nameEn: District.nameEn,
      nameAr: District.nameAr,
      cityId: District.cityId
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
    formData.append('CityId', formValue.cityId?.toString() || '');

    if (this.selectedImageFile) {
      formData.append('Image', this.selectedImageFile);
    }

    if (this.formMode === 'Create') {
      this.createDistrict(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updateDistrict(this.editingId, formData);
    }
  }

  private createDistrict(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('District created successfully!');
      },
      error: err => {
        console.error('Error creating District:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updateDistrict(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('District updated successfully!');
      },
      error: err => {
        console.error('Error updating District:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(District: District): void {
    this.service.updateStatus(District.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('District status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteDistrict(id: number): void {
    if (confirm('Are you sure you want to delete this District?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('District deleted successfully!');
        },
        error: err => {
          console.error('Error deleting District:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
