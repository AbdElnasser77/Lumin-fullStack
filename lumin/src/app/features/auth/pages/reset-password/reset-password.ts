import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.html',
  imports: [ReactiveFormsModule, CommonModule]
})
export class ResetPasswordComponent implements OnInit {
  resetForm!: FormGroup;
  isLoading: boolean = false;
  token: string = '';
  passwordStrength = {
    minLength: false,
    uppercase: false,
    number: false,
    symbol: false,
  };

  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      if (!this.token) {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Invalid or missing reset token', life: 3000 });
        this.router.navigate(['/auth/sign-in']);
      }
    });

    this.resetForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordMatchValidator });

    this.resetForm.get('newPassword')?.valueChanges.subscribe(value => {
      this.updatePasswordStrength(value);
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = control.get('newPassword');
    const confirmPassword = control.get('confirmPassword');

    if (!newPassword || !confirmPassword) return null;

    return newPassword.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  updatePasswordStrength(password: string): void {
    this.passwordStrength.minLength = password.length >= 8;
    this.passwordStrength.uppercase = /[A-Z]/.test(password);
    this.passwordStrength.number = /\d/.test(password);
    this.passwordStrength.symbol = /[!@#$]/.test(password);
  }

  isPasswordStrong(): boolean {
    return Object.values(this.passwordStrength).every(v => v === true);
  }

  onSubmit(): void {
    if (this.resetForm.invalid || !this.isPasswordStrong()) {
      this.resetForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const newPassword = this.resetForm.get('newPassword')?.value;

    this.authService.resetPassword({ token: this.token, newPassword }).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Password reset successfully', life: 3000 });
        this.router.navigate(['/auth/sign-in']);
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error.message || 'Failed to reset password', life: 3000 });
        this.isLoading = false;
      }
    });
  }
}
