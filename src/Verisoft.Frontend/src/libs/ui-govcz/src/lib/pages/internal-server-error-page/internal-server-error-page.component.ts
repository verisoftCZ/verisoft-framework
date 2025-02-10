import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';

@Component({
  selector: 'v-internal-server-error-page',
  standalone: true,
  imports: [CommonModule, GovDesignSystemModule],
  templateUrl: './internal-server-error-page.component.html',
  styleUrl: './internal-server-error-page.component.scss',
})
export class InternalServerErrorPageComponent {
  navigationBack() {
    history.back();
  }
}
