import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
@Component({
  selector: 'v-bad-request-page',
  standalone: true,
  imports: [CommonModule, GovDesignSystemModule],
  templateUrl: './bad-request-page.component.html',
  styleUrl: './bad-request-page.component.scss'
})
export class BadRequestPageComponent {
  navigationBack() {
    history.back();
  }
}
