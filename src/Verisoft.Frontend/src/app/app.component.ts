import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  imports: [
    RouterModule,
],
  selector: 'v-app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
}
