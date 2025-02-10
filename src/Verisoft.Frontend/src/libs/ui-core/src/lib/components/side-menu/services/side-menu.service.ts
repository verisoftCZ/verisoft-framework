import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MenuItem, SIDE_MENU_STATE_TOKEN } from '../side-menu.model';

@Injectable({ providedIn: 'root' })
export class SideMenuService {
  private _menuItems = new BehaviorSubject<MenuItem[]>([]);
  private stateToken = inject(SIDE_MENU_STATE_TOKEN);

  menuItems$: Observable<MenuItem[]> = this._menuItems.asObservable();
  menuMinimalized = false;

  setMenu(items: MenuItem[]): void {
    this.resetSidemenuState(items);
    this._menuItems.next(items);
  }

  saveMinimalizedState(minimalized: boolean): void {
    this.stateToken.minimalized = minimalized;
    localStorage.setItem('SideMenuStateToken', JSON.stringify(this.stateToken));
  }

  saveExpandedState(item: MenuItem): void {
    const expanded = this.stateToken.expanded?.find(
      (i: string) => i === item.label
    );

    if (!expanded) {
      this.stateToken.expanded?.push(item.label);
      localStorage.setItem(
        'SideMenuStateToken',
        JSON.stringify(this.stateToken)
      );
      return;
    }

    this.stateToken.expanded = this.stateToken.expanded?.filter(
      (i: string) => i !== item.label
    );

    localStorage.setItem('SideMenuStateToken', JSON.stringify(this.stateToken));
  }

  private resetSidemenuState(items: MenuItem[]) {
    if (typeof this.stateToken === 'string') {
      this.stateToken = JSON.parse(this.stateToken);
    }

    this.menuMinimalized = this.stateToken.minimalized;
    const localStorageValue = this.stateToken.expanded;

    if (!localStorageValue) return;
    if (items) {
      for (let index = 0; index < items.length; index++) {
        const element = items[index];
        element.expanded = !!localStorageValue.find(
          (i: string) => i === element.label
        );
      }
    }
  }
}
