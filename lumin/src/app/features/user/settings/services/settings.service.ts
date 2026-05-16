import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserProfile, NotificationPreferences, AppearanceSettings } from '../models/settings.model';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  constructor(private http: HttpClient) {}

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>('/api/profile');
  }

  updateProfile(profile: Partial<UserProfile>): Observable<UserProfile> {
    return this.http.put<UserProfile>('/api/profile', profile);
  }

  getNotificationPrefs(): Observable<NotificationPreferences> {
    return this.http.get<NotificationPreferences>('/api/profile/notifications');
  }

  updateNotificationPrefs(prefs: NotificationPreferences): Observable<NotificationPreferences> {
    return this.http.put<NotificationPreferences>('/api/profile/notifications', prefs);
  }

  getAppearance(): Observable<AppearanceSettings> {
    return this.http.get<AppearanceSettings>('/api/profile/appearance');
  }

  updateAppearance(settings: AppearanceSettings): Observable<AppearanceSettings> {
    return this.http.put<AppearanceSettings>('/api/profile/appearance', settings);
  }
}
