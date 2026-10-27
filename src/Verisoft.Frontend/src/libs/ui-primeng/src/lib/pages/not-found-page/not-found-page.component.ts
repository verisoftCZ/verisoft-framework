
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ButtonComponent } from '../../components/button';

@Component({
    selector: 'v-not-found-page',
    imports: [ButtonComponent],
    templateUrl: './not-found-page.component.html',
    styleUrl: './not-found-page.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotFoundPageComponent {
  navigationBack() {
    history.back();
  }
}
