import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';

@Component({
  selector: 'v-not-found-page',
  standalone: true,
  imports: [CommonModule, GovDesignSystemModule],
  templateUrl: './not-found-page.component.html',
  styleUrl: './not-found-page.component.scss',
})
export class NotFoundPageComponent {
  navigationBack() {
    history.back();
  }
}
