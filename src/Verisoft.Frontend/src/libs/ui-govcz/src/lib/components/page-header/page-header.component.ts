import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ContentChild,
  OnInit,
} from '@angular/core';
import { PAGE_HEADER_COMPONENT_TOKEN, PageHeaderCore, PageHeaderCoreComponent } from '@verisoft/ui-core';
import { ButtonComponent } from '../button';
import { ActionButtonGroupComponent } from '../shared-components';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';

@Component({
  selector: 'v-page-header',
  standalone: true,
  imports: [CommonModule, ButtonComponent, GovDesignSystemModule],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: PAGE_HEADER_COMPONENT_TOKEN, useExisting: PageHeaderComponent },
  ],
})
export class PageHeaderComponent
  extends PageHeaderCoreComponent
  implements OnInit, PageHeaderCore
{
  @ContentChild(ActionButtonGroupComponent)
  buttonGroup?: ActionButtonGroupComponent;
}