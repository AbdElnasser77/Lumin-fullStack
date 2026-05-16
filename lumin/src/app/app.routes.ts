import { Routes } from '@angular/router';
import { authGuard } from './features/auth/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'auth',
    loadComponent: () => import('./layouts/auth-layout/auth-layout').then(m => m.AuthLayoutComponent),
    loadChildren: () => import('./features/auth/pages/auth.routes').then(m => m.authRoutes),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layouts/user-layout/user-layout').then(m => m.UserLayoutComponent),
    loadChildren: () => import('./features/user.routes').then(m => m.userRoutes),
  },
];
