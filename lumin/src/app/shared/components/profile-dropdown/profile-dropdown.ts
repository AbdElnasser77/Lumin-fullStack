import { Component, output } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-profile-dropdown',
  templateUrl: './profile-dropdown.html',
  imports: [RouterLink],
})
export class ProfileDropdownComponent {
  close = output<void>();
}
