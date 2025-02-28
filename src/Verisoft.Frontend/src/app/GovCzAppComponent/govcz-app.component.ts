import { CommonModule } from '@angular/common';
import { Component, inject, ViewEncapsulation } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DOCS_CONFIG } from '@verisoft/demo-documentation-ui';
import { SideMenuService } from '@verisoft/ui-core';
import { GovInitService } from '@verisoft/ui-govcz';
import {
  HeaderComponent,
  SideMenuComponent,
  BreadcrumbComponent,
  ConfirmDialogComponent,
} from '@verisoft/ui-govcz';
import { getMenuItems } from '../app.config';
@Component({
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    SideMenuComponent,
    BreadcrumbComponent,
    ConfirmDialogComponent,
    HeaderComponent,
  ],
  providers: [
    {
      provide: DOCS_CONFIG,
      useValue: { isPrimeng: false },
    },
  ],
  selector: 'govcz-app',
  templateUrl: './govcz-app.component.html',
  styleUrl: './govcz-app.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class GovCzAppComponent {
  minimalized!: boolean;
  menuService = inject(SideMenuService);
  govInitService = inject(GovInitService);
  items = getMenuItems(false);

  protected menuMinimalized($event: boolean) {
    this.minimalized = $event;
  }
}
