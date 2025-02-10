import { Inject, Injectable } from '@angular/core';
import { SETTINGS_MENU } from '@verisoft/ui-core';
import { MenuItem } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class HeaderProviderService {
  constructor(@Inject(SETTINGS_MENU) private settingsMenu: MenuItem[]) {}
  menu: MenuItem[] = [];

  init(): void {
    this.menu = this.settingsMenu;
  }
}
