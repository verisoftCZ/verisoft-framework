import { Injectable, Optional, Inject } from '@angular/core';
import { MENU_TOKEN, MenuItem } from '../side-menu.model';
import { SideMenuService } from './side-menu.service';

@Injectable()
export class SideMenuProviderService {
  constructor(
    @Optional() @Inject(MENU_TOKEN) readonly menu: MenuItem[] = [],
    readonly menuService: SideMenuService
  ) {
    menuService.setMenu(menu);
  }
}
