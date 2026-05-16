import { Routes } from '@angular/router';
import { publicGuard } from '../guards/public.guard';

export const authRoutes: Routes = [
  { path: 'sign-in',         canActivate: [publicGuard], loadComponent: () => import('./sign-in/sign-in').then(m => m.SignInComponent) },
  { path: 'sign-up',         canActivate: [publicGuard], loadComponent: () => import('./sign-up/sign-up').then(m => m.SignUpComponent) },
  { path: 'forgot-password', canActivate: [publicGuard], loadComponent: () => import('./forgot-password/forgot-password').then(m => m.ForgotPasswordComponent) },
  { path: 'reset-password',  canActivate: [publicGuard], loadComponent: () => import('./reset-password/reset-password').then(m => m.ResetPasswordComponent) },
  { path: 'verify-otp',      canActivate: [publicGuard], loadComponent: () => import('./verify-otp/verify-otp').then(m => m.VerifyOtpComponent) },
  { path: '', redirectTo: 'sign-in', pathMatch: 'full' },
];
