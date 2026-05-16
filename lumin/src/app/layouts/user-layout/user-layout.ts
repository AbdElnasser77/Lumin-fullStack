import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar';

@Component({
  selector: 'app-user-layout',
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './user-layout.html',
})
export class UserLayoutComponent {}
