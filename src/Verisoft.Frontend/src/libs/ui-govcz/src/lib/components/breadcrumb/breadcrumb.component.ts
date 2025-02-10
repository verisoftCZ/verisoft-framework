import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  BREADCRUMB_COMPONENT_TOKEN,
  BreadcrumbCoreComponent
} from '@verisoft/ui-core';

@Component({
  selector: 'v-breadcrumb',
  standalone: true,
  imports: [
    CommonModule,
    GovDesignSystemModule,
    RouterModule
  ],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
  providers: [
    {
      provide: BREADCRUMB_COMPONENT_TOKEN,
      useExisting: BreadcrumbComponent
    },
  ],
})
export class BreadcrumbComponent extends BreadcrumbCoreComponent { }
