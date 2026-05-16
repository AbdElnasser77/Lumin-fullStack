import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.html',
})
export class SettingsComponent {
  activeSection = 'profile';

  groups = [
    {
      label: 'Account',
      items: [
        { id: 'profile',       label: 'Profile' },
        { id: 'account',       label: 'Account & security' },
        { id: 'notifications', label: 'Notifications' },
      ],
    },
    {
      label: 'Workspace',
      items: [
        { id: 'appearance',   label: 'Appearance' },
        { id: 'tasks',        label: 'Tasks' },
        { id: 'habits',       label: 'Habits' },
        { id: 'budget',       label: 'Budget & money' },
        { id: 'integrations', label: 'Integrations' },
      ],
    },
    {
      label: 'Advanced',
      items: [
        { id: 'data',    label: 'Data & export' },
        { id: 'billing', label: 'Plan & billing' },
      ],
    },
  ];
}
