import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { PasswordStrengthUI } from "../../components/password-strength-ui/password-strength-ui";
import { ValidationMessageComponent } from '../../../../shared/components/validation-message/validation-message';
import { switchMap } from 'rxjs';
import { Spinner } from "../../../../shared/components/spinner/spinner";
import { PasswordModule } from 'primeng/password';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.html',
  styleUrl: './sign-up.css',
  imports: [ReactiveFormsModule, RouterLink, PasswordStrengthUI, ValidationMessageComponent, Spinner, PasswordModule]
})
export class SignUpComponent implements OnInit {

  private authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  signupForm!: FormGroup;
  isloading = signal(false);
  errorMessage:string = '';

  ngOnInit() {
    this.signupForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
        lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.pattern(/(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/)]],
        rePassword: ['', [Validators.required]],
        checkTerms: [false, [Validators.requiredTrue]]
      }, {
      validators: this.rePasswordValidator
    }
    )
  }

  rePasswordValidator(control: AbstractControl) {
    let password = control.get('password')?.value;
    let rePassword = control.get('rePassword')?.value;

    if (password !== rePassword) {
      return { mismatch: true };
    }

    return null;
  }

  submitForm() {
    this.errorMessage = '';
    if (this.signupForm.valid) {
      this.isloading.set(true);
      const { rePassword, ...formData } = this.signupForm.value;
      this.authService.signUp(formData).pipe(
        switchMap(() =>
          this.authService.sendEmailConfirmation(this.signupForm.get('email')?.value)
        )
      ).subscribe({
        next: () => {
          this.router.navigate(['auth/verify-otp'], {
            state: { email: this.signupForm.get('email')?.value }
          });
        },
        error: (err) => {
          this.isloading.set(false);
          this.errorMessage = err.error.message;
        }
      });
    } else {
      this.signupForm.markAllAsTouched();
      this.isloading.set(false);
    }
  }
}
