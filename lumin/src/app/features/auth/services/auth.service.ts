import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { SignupRequest } from '../models/signup/signup-request';
import { VerifyOTPRes } from '../models/verify-otp/verify-otp-res';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = `${environment.baseUrl}/api/auth`;
  private http = inject(HttpClient);

  signUp(data:SignupRequest): Observable<any> {
    return this.http.post(`${this.base}/signup`, data);
  }

  signIn(data: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.base}/signin`, data);
  }

  forgotPassword(data: { email: string; redirectUrl: string }): Observable<any> {
    return this.http.post(`${this.base}/forgot-password`, data);
  }

  sendEmailConfirmation(email: string ): Observable<any> {
    return this.http.post(`${this.base}/send-email-confirmation`, {email});
  }

  confirmEmail(data: { email: string; otp: string }): Observable<VerifyOTPRes> {
    return this.http.post<VerifyOTPRes>(`${this.base}/confirm-email`, data);
  }

  resetPassword(data: { token: string; newPassword: string }): Observable<any> {
    return this.http.post(`${this.base}/reset-password`, data);
  }

  refresh(data: { refreshToken: string }): Observable<any> {
    return this.http.post(`${this.base}/refresh`, data);
  }

  signOut(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
