import { Component, input } from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'validation-message',
  imports: [],
  template: `
    @for (msg of messages; track msg) {
      <p class="text-[12px] text-danger mt-1">{{ msg }}</p>
    }
  `
})
export class ValidationMessageComponent {
  control = input<AbstractControl | null>(null);
  label = input<string>('This field');
  groupErrors = input<ValidationErrors | null>(null);

  get messages(): string[] {
    const ctrl = this.control();
    if (!ctrl || !(ctrl.dirty || ctrl.touched)) return [];

    const e: ValidationErrors = { ...(ctrl.errors ?? {}), ...(ctrl.dirty ? (this.groupErrors() ?? {}) : {}) };

    const msgs: string[] = [];
    const lbl = this.label();

    if (e['required']) msgs.push(`${lbl} is required.`);
    if (e['minlength']) msgs.push(`${lbl} must be at least ${e['minlength'].requiredLength} characters.`);
    if (e['maxlength']) msgs.push(`${lbl} must be at most ${e['maxlength'].requiredLength} characters.`);
    if (e['email']) msgs.push('Please enter a valid email address.');
    if (e['mismatch']) msgs.push('Passwords do not match.');
    if (e['requiredTrue']) msgs.push('You must accept the terms and conditions.');

    return msgs;
  }
}
