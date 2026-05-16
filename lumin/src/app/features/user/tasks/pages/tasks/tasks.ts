import { Component } from '@angular/core';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.html',
})
export class TasksComponent {
  currentDate!: string;

  ngOnInit(){
    this.currentDate = new Date().toLocaleDateString('en-US', {
      weekday: 'long',
      month: 'long',
      day: 'numeric'
    });

  }

}
