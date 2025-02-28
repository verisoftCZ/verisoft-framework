import { CommonModule } from '@angular/common';
import {
  Component,
} from '@angular/core';
import {
  BREADCRUMB_COMPONENT_TOKEN,
  BreadcrumbCoreComponent,
} from '@verisoft/ui-core';
import { BreadcrumbModule } from 'primeng/breadcrumb';

@Component({
  selector: 'v-breadcrumb',
  standalone: true,
  imports: [CommonModule, BreadcrumbModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
  providers: [
    { 
      provide: BREADCRUMB_COMPONENT_TOKEN,
      useExisting: BreadcrumbComponent
    },
  ],
})
export class BreadcrumbComponent extends BreadcrumbCoreComponent { 
}
