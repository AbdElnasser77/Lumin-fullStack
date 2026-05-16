export interface UserProfile {
  firstName: string;
  lastName: string;
  displayName?: string;
  pronouns?: string;
  timeZone?: string;
  workplace?: string;
  role?: string;
  bio?: string;
}

export interface NotificationPreferences {
  email: boolean;
  push: boolean;
  inApp: boolean;
}

export type AppTheme    = 'Paper' | 'Vellum' | 'Charcoal';
export type AppDensity  = 'Cozy' | 'Comfortable' | 'Compact';

export interface AppearanceSettings {
  theme: AppTheme;
  accent: string;
  density: AppDensity;
  reducedMotion: boolean;
}
