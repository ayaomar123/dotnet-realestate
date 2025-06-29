import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { NotificationService } from '../../../shared/services/notification.service';
import { Status } from './interfaces/status';
import { StatusService } from './services/status.service';
import { CommonModule } from '@angular/common';
import { ModalComponent } from "../../../shared/components/modal/modal.component";
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";

@Component({
  selector: 'app-status',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, ImageUploadComponent],
  templateUrl: './status.component.html',
  styleUrl: './status.component.css'
})
export class StatusComponent {
  private readonly service = inject(StatusService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: Status[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Statuses';

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
      nameAr: ['', Validators.required],
      color: ['', Validators.required],
      backgroundColor: ['', Validators.required],
      sort: ['', Validators.required],
    });
  }

  private subscribeToNotifications(): void {
    this.notificationSubscription = this.toastr.notifications$.subscribe(notification => {
      this.notifications.push(notification);
      setTimeout(() => this.notifications.shift(), 3000);
    });
  }

  private loadData(): void {
    this.service.get().subscribe((res: General<Status[]>) => {
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

  editStatus(Status: Status): void {
    this.formMode = 'Edit';
    this.editingId = Status.id;
    this.form.patchValue({
      nameEn: Status.nameEn,
      nameAr: Status.nameAr,
      color: Status.color,
      backgroundColor: Status.backgroundColor,
      sort: Status.sort,
    });
    this.isModalOpen = true;
  }

  onSubmit(): void {
    console.log('Form submitted:', this.form);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const formData = new FormData();
    formData.append('NameEn', formValue.nameEn || '');
    formData.append('NameAr', formValue.nameAr || '');
    formData.append('color', formValue.color || '');
    formData.append('backgroundColor', formValue.backgroundColor || '');
    formData.append('sort', formValue.sort || '');

    if (this.selectedImageFile) {
      formData.append('Image', this.selectedImageFile);
    }
    if (this.formMode === 'Create') {
      this.createStatus(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updateStatus(this.editingId, formData);
    }
  }

  private createStatus(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Status created successfully!');
      },
      error: err => {
        console.error('Error creating Status:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updateStatus(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Status updated successfully!');
      },
      error: err => {
        console.error('Error updating Status:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(Status: Status): void {
    this.service.updateStatus(Status.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('Status status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteStatus(id: number): void {
    if (confirm('Are you sure you want to delete this Status?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('Status deleted successfully!');
        },
        error: err => {
          console.error('Error deleting Status:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
