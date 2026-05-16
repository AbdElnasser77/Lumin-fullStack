import { Routes } from '@angular/router';
import { authGuard } from './auth/guards/auth.guard';

export const userRoutes: Routes = [
  { path: 'dashboard', canActivate: [authGuard], loadComponent: () => import('./user/dashboard/pages/dashboard/dashboard').then(m => m.DashboardComponent) },
  { path: 'tasks',     canActivate: [authGuard], loadComponent: () => import('./user/tasks/pages/tasks/tasks').then(m => m.TasksComponent) },
  { path: 'habits',    canActivate: [authGuard], loadComponent: () => import('./user/habits/pages/habits/habits').then(m => m.HabitsComponent) },
  { path: 'budget',    canActivate: [authGuard], loadComponent: () => import('./user/budget/pages/budget/budget').then(m => m.BudgetComponent) },
  { path: 'settings',  canActivate: [authGuard], loadComponent: () => import('./user/settings/pages/settings/settings').then(m => m.SettingsComponent) },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
];
