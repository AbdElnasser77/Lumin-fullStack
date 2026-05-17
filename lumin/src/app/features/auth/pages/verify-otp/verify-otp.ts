import { Component, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputOtpModule } from 'primeng/inputotp';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { CountdownComponent } from 'ngx-countdown';
import { AuthService } from '../../services/auth.service';
import { Spinner } from '../../../../shared/components/spinner/spinner';
@Component({
  selector: 'app-verify-otp',
  templateUrl: './verify-otp.html',
  imports: [CommonModule, RouterLink, InputOtpModule, MessageModule, ToastModule, ButtonModule, FormsModule, ReactiveFormsModule, CountdownComponent, Spinner],
  standalone: true,
  styleUrl:'./verify-otp.css'
})
export class VerifyOtpComponent {
  email: string = history.state.email;
  value: string = '';
  resendTimer: number = 60;
  canResend: boolean = false;
  isLoading = signal(false);

  private messageService = inject(MessageService);
  private authService = inject(AuthService);
  private router = inject(Router);

  @ViewChild('countDown') countDown!: CountdownComponent;

  onSubmit(form: any) {
    this.isLoading.set(true);
    if (form.valid && form.value.value.length === 6) {
      const data = {
        email: this.email,
        otp: form.value.value
      }
      this.authService.confirmEmail(data).subscribe({
        next: (res) => {
          this.authService.setToken(res.token);
          this.router.navigate(['/dashboard']);
          this.isLoading.set(false);
        },
        error: (err) => {
          console.log(err);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error.message, life: 3000 });
          this.isLoading.set(false);
        }
      })
    } else {
      this.messageService.add({
        severity: 'error', summary: 'Invalid OTP', detail: 'Please enter all OTP digits before submitting', life: 3000
      });
      this.isLoading.set(false);

    }
  }

  onTimerDone(event: any) {
    if (event.action == "done") {
      this.canResend = true;
    }
  }

  resetOtpTimer() {
    this.countDown.restart();
    this.canResend = false;

    this.authService.sendEmailConfirmation(this.email).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: `OTP Sent to ${this.email}`, life: 3000 });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: `Something went wrong`, detail: err.error.message, life: 3000 });
      }
    });
  }
}
