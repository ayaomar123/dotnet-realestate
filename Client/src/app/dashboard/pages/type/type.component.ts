import { Component, inject, Type as AngularType } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { NotificationService } from '../../../shared/services/notification.service';
import { TypeService } from './services/type.service';
import { Type } from './interfaces/type';
import { CommonModule } from '@angular/common';
import { ModalComponent } from "../../../shared/components/modal/modal.component";
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";

@Component({
  selector: 'app-type',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, ImageUploadComponent],
  templateUrl: './type.component.html',
  styleUrl: './type.component.css'
})
export class TypeComponent {
  private readonly service = inject(TypeService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: Type[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Types';

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
    this.service.get().subscribe((res: General<Type[]>) => {
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

  editType(Type: Type): void {
    this.formMode = 'Edit';
    this.editingId = Type.id;
    this.form.patchValue({
      nameEn: Type.nameEn,
      nameAr: Type.nameAr
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
      this.createType(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updateType(this.editingId, formData);
    }
  }

  private createType(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Type created successfully!');
      },
      error: err => {
        console.error('Error creating Type:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updateType(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Type updated successfully!');
      },
      error: err => {
        console.error('Error updating Type:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(Type: Type): void {
    this.service.updateStatus(Type.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('Type status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteType(id: number): void {
    if (confirm('Are you sure you want to delete this Type?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('Type deleted successfully!');
        },
        error: err => {
          console.error('Error deleting Type:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
