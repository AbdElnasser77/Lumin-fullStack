import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { PasswordStrengthUI } from "../../components/password-strength-ui/password-strength-ui";
import { ValidationMessageComponent } from '../../../../shared/components/validation-message/validation-message';
import { switchMap } from 'rxjs';
import { Spinner } from "../../../../shared/components/spinner/spinner";

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.html',
  imports: [ReactiveFormsModule, RouterLink, PasswordStrengthUI, ValidationMessageComponent, Spinner]
})
export class SignUpComponent implements OnInit {

  private authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  signupForm!: FormGroup;
  isloading: boolean = false;

  ngOnInit() {
    this.signupForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
        lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.pattern('(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$')]],
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
    if (this.signupForm.valid) {
      this.isloading = true;
      this.authService.signUp(this.signupForm.value).pipe(
        switchMap(() =>
          this.authService.sendEmailConfirmation(this.signupForm.get('email')?.value)
        )
      ).subscribe({
        next: (res) => {
          this.router.navigate(['auth/verify-otp'], {
            state: { email: this.signupForm.get('email')?.value }
          })
        }
        
      })
    } else {
      this.signupForm.markAllAsTouched()
      this.isloading = false;

    }

  }
}
