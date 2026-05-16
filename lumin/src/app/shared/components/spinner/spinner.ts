import { Component } from '@angular/core';
import { ProgressSpinnerModule, ProgressSpinner } from 'primeng/progressspinner';

@Component({
  selector: 'spinner',
  imports: [ProgressSpinner],
  templateUrl: './spinner.html',
  styleUrl: './spinner.css',
})
export class Spinner {

}
