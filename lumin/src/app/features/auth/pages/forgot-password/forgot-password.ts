import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.html',
  imports: [ReactiveFormsModule, CommonModule]
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm!: FormGroup;
  isLoading: boolean = false;
  step: number = 1;
  expiryTime: string = '15:00';

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.forgotPasswordForm.invalid) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const email = this.forgotPasswordForm.get('email')?.value;
    const resetPageUrl = `${window.location.origin}/auth/reset-password`;

    this.authService.forgotPassword({ email, redirectUrl: resetPageUrl }).subscribe({
      next: () => {
        this.step = 2;
        this.isLoading = false;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error.message || 'Failed to send recovery link', life: 3000 });
        this.isLoading = false;
      }
    });
  }

  onResend() {
    this.step = 1;
  }

  backToSignIn() {
    this.router.navigate(['/auth/sign-in']);
  }

  changeEmail() {
    this.step = 1;
    this.forgotPasswordForm.reset();
  }

  getEmailDisplay(): string {
    return this.forgotPasswordForm.get('email')?.value || 'your email';
  }
}
