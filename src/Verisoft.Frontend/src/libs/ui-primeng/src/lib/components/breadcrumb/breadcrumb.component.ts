
import {
  Component,
} from '@angular/core';
import {
  BREADCRUMB_COMPONENT_TOKEN,
  BreadcrumbCoreComponent,
} from '@verisoft/ui-core';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ConvertMenuItemPipe } from './breadcrumb.pipe';

@Component({
    selector: 'v-breadcrumb',
    imports: [
    BreadcrumbModule,
    ConvertMenuItemPipe
],
    templateUrl: './breadcrumb.component.html',
    styleUrl: './breadcrumb.component.scss',
    providers: [
        {
            provide: BREADCRUMB_COMPONENT_TOKEN,
            useExisting: BreadcrumbComponent
        },
    ]
})
export class BreadcrumbComponent extends BreadcrumbCoreComponent { }
