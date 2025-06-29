import { Component, inject, OnInit } from '@angular/core';
import { User } from './interfaces/profile';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  form!: FormGroup;
  user!: User;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({
      fNameEn: ['جون', Validators.required],
      lNameEn: ['John', Validators.required],
      fNameAr: ['دو', Validators.required],
      lNameAr: ['Doe', Validators.required],
      email: [{ value: 'admin@example.com', disabled: true }],
      phone: ['0500000000', Validators.required],
      username: ['admin', Validators.required],
      role: ['Admin', Validators.required],
    });

    this.user = {
      id: '1',
      userName: 'admin',
      email: 'admin@example.com',
      phoneNumber: '0500000000',
      fNameEn: 'John',
      lNameEn: 'Doe',
      fNameAr: 'جون',
      lNameAr: 'دو',
      role: 'Admin',
      image: null
    };

  }
  previewUrl: string | null = null;

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      this.previewUrl = reader.result as string;
    };
    reader.readAsDataURL(file);
  }


}
