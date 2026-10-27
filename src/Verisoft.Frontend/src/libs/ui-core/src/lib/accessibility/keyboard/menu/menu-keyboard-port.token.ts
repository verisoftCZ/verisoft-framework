import { InjectionToken } from '@angular/core';
import { MenuKeyboardFocusA11y } from './menu-keyboard.model';

export interface MenuKeyboardPort {
  open(): boolean | Promise<boolean>;
  close(restoreFocus: boolean): void | Promise<void>;
  focusTrigger?(triggerHost: HTMLElement, a11y?: MenuKeyboardFocusA11y): void;
  focusItem?(itemHost: HTMLElement, a11y?: MenuKeyboardFocusA11y): void;
  applyTriggerAccessibility?(
    triggerHost: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void;
  applyItemAccessibility?(
    itemHost: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void;
}

export const MENU_KEYBOARD_PORT = new InjectionToken<MenuKeyboardPort>(
  'MENU_KEYBOARD_PORT',
);
