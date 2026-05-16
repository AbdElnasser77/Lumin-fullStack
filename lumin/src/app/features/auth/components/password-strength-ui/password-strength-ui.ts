import { Component, Input } from '@angular/core';

@Component({
  selector: 'password-strength',
  imports: [],
  templateUrl: './password-strength-ui.html',
})
export class PasswordStrengthUI {
  @Input() password: string = '';

  passwordStrength = 0;

  ngOnChanges() {
    this.checkPasswordStrength();
  }

  checkPasswordStrength() {
    let strength = 0;
  
    if (this.password.length >= 8) strength++;
    if (/[A-Z]/.test(this.password)) strength++;
    if (/[a-z]/.test(this.password) && /\d/.test(this.password)) strength++;
    if (/[\W_]/.test(this.password)) strength++;

    this.passwordStrength = strength;
  }
}
