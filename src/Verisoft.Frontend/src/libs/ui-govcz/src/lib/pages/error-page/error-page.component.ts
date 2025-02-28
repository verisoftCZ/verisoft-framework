import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { ErrorDetails, ERROR_MAP } from './error-page.constants';

@Component({
  selector: 'v-error-page',
  standalone: true,
  imports: [CommonModule, GovDesignSystemModule],
  templateUrl: './error-page.component.html',
  styleUrl: './error-page.component.scss'
})
export class ErrorPageComponent {
  @Input() url!: string;
  @Input() code = 404;

  constructor(private router: Router) {}

  get errorDetails(): ErrorDetails {
    return ERROR_MAP[this.code];
  }

  navigateToUrl() {
    if (this.url) {
      this.router.navigate([this.url]);
    }
    else {
      history.back();
    }
  }
}
