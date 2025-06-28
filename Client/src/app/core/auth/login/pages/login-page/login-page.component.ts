import { Component, DestroyRef, inject } from '@angular/core';
import { AuthService } from '../../../auth.service';
import { Login } from '../../interfaces';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent {
  private readonly authSvc = inject(AuthService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  loginForm!: FormGroup;
  errorMessage: string | null = null;

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['Admin@gmail.com', [Validators.required, Validators.email]],
      password: ['Admin123!', Validators.required]
    });
  }

  onLoginFormSubmitted() {
    if (!this.loginForm.valid) {
      return;
    }

    this.authSvc.login(this.loginForm.value as Login).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: () => {
      },
      error: (error) => {
        this.errorMessage = error.message || 'Login failed. Please try again.';
      }
    });
  }
}
