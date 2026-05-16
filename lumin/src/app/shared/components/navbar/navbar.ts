import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ProfileDropdownComponent } from '../../components/profile-dropdown/profile-dropdown';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  imports: [RouterLink, ProfileDropdownComponent, RouterLinkActive],
})
export class NavbarComponent {
  showDropdown = false;
}
