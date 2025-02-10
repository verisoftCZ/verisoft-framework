import { CommonModule } from '@angular/common';
import { Component, inject, ViewEncapsulation } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DOCS_CONFIG } from '@verisoft/demo-documentation-ui';
import { SideMenuService } from '@verisoft/ui-core';
import {
  HeaderComponent,
  SideMenuComponent,
  BreadcrumbComponent,
  SnackbarComponent,
  ConfirmDialogComponent,
} from '@verisoft/ui-primeng';
import { getMenuItems } from '../app.config';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    SideMenuComponent,
    BreadcrumbComponent,
    SnackbarComponent,
    ConfirmDialogComponent,
    HeaderComponent,
  ],
  providers: [
    {
      provide: DOCS_CONFIG,
      useValue: { isPrimeng: true },
    },
  ],
  selector: 'primeng-app',
  templateUrl: './primeng-app.component.html',
  styleUrl: './primeng-app.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class PrimengAppComponent {
  minimalized!: boolean;
  menuService = inject(SideMenuService);
  items = getMenuItems(true);

  protected menuMinimalized($event: boolean) {
    this.minimalized = $event;
  }
}
