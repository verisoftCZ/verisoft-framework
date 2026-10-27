
import {
  ChangeDetectionStrategy,
  Component,
  Input,
} from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { TranslateModule } from '@ngx-translate/core';
import { PageHeaderCore } from '@verisoft/ui-core';

@Component({
    selector: 'v-page-header',
    imports: [
    GovDesignSystemModule,
    TranslateModule
],
    templateUrl: './page-header.component.html',
    styleUrl: './page-header.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PageHeaderComponent implements PageHeaderCore {
  @Input() title = '';

  @Input() subtitle: string | undefined;

  @Input() showBackButton: boolean | undefined;
}
