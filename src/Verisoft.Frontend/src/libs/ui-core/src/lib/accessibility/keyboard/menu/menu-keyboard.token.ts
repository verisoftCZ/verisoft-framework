import { InjectionToken } from '@angular/core';
import { MenuKeyboardFacade } from './menu-keyboard.facade';
import { MenuKeyboardItem } from './menu-keyboard.model';

export const MENU_KEYBOARD = new InjectionToken<
  MenuKeyboardFacade<MenuKeyboardItem>
>('MENU_KEYBOARD');
