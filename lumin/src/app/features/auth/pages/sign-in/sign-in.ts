import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Spinner } from "../../../../shared/components/spinner/spinner";
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { Toast } from "primeng/toast";

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.html',
  imports: [RouterLink, FormsModule, ReactiveFormsModule, Spinner, Toast],
})
export class SignInComponent implements OnInit {

  signInForm!: FormGroup;
  isLoading: boolean = false;

  private fb = inject(FormBuilder)
  private authService = inject(AuthService)
  private router = inject(Router)
  private messageService = inject(MessageService)

  ngOnInit() {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    })
  }

  signIn() {
    this.isLoading = true;
    if (this.signInForm.valid) {
      this.authService.signIn(this.signInForm.value).subscribe({
        next: (res) => {
          this.authService.setToken(res.token);
          this.router.navigate(['/dashboard']);
          this.isLoading = false;
        },
        error: (err) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error.message, life: 3000 });
          this.isLoading = false;
        }
      })
    } else {
      this.isLoading = false;
      this.signInForm.markAllAsTouched();
    }
  }
}
