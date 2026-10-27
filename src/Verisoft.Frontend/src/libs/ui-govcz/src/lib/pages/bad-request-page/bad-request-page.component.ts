
import { Component } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
@Component({
    selector: 'v-bad-request-page',
    imports: [GovDesignSystemModule],
    templateUrl: './bad-request-page.component.html',
    styleUrl: './bad-request-page.component.scss'
})
export class BadRequestPageComponent {
  navigationBack() {
    history.back();
  }
}
