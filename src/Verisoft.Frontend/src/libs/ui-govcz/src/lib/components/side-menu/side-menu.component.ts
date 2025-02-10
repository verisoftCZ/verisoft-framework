import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { RouterModule } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  MenuItem,
  SIDE_MENU_COMPONENT_TOKEN,
  SideMenuCore,
  SideMenuService,
} from '@verisoft/ui-core';

@Component({
  selector: 'v-side-menu',
  standalone: true,
  imports: [CommonModule, RouterModule, GovDesignSystemModule],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.scss',
  providers: [
    {
      provide: SIDE_MENU_COMPONENT_TOKEN,
      useExisting: SideMenuComponent,
    },
  ],
})
export class SideMenuComponent implements SideMenuCore {
  menuService = inject(SideMenuService);

  @Input() items: MenuItem[] = [];

  @Input() logoUrl = '';

  @Input() userName!: string;

  @Input() userRole!: any | any[] | undefined;

  @Output() minimalized = new EventEmitter<boolean>();

  @Output() itemSelected = new EventEmitter<MenuItem>();
}
