import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CategoryService } from './services/category.service';
import { Category } from './interfaces/category';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { ModalComponent } from '../../../shared/components/modal/modal.component';
import { NotificationService } from '../../../shared/services/notification.service';
import { ImageUploadComponent } from "../../../shared/components/image-upload/image-upload.component";

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent, ImageUploadComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  private readonly service = inject(CategoryService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: Category[] = [];
  form!: FormGroup;

  isModalOpen = false;
  formMode: 'Create' | 'Edit' = 'Create';
  editingId: number | null = null;

  selectedImageFile?: File;
  title: string = 'Categories';

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
    this.service.get().subscribe((res: General<Category[]>) => {
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

  editCategory(category: Category): void {
    this.formMode = 'Edit';
    this.editingId = category.id;
    this.form.patchValue({
      nameEn: category.nameEn,
      nameAr: category.nameAr
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
      this.createCategory(formData);
    } else if (this.formMode === 'Edit' && this.editingId) {
      formData.append('Id', this.editingId.toString());
      this.updateCategory(this.editingId, formData);
    }
  }

  private createCategory(formData: FormData): void {
    this.service.create(formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Category created successfully!');
      },
      error: err => {
        console.error('Error creating category:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  private updateCategory(id: number, formData: FormData): void {
    this.service.update(id, formData).subscribe({
      next: () => {
        this.loadData();
        this.closeModal();
        this.toastr.success('Category updated successfully!');
      },
      error: err => {
        console.error('Error updating category:', err);
        this.toastr.error('Data not saved');
      }
    });
  }

  changeStatus(category: Category): void {
    this.service.updateStatus(category.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('Category status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteCategory(id: number): void {
    if (confirm('Are you sure you want to delete this category?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('Category deleted successfully!');
        },
        error: err => {
          console.error('Error deleting category:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

  onImageSelected(file: File): void {
    this.selectedImageFile = file;
  }
}
