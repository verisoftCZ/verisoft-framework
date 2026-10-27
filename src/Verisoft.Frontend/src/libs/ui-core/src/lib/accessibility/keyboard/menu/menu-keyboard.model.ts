import { KeyboardKeyList } from '../core/keyboard-key.model';

export interface MenuKeyboardItem {
  disabled?: boolean;
}

export interface MenuKeyboardFocusA11y {
  role?: string;
  tabIndex?: number;
  ariaHaspopup?: string;
  ariaExpanded?: string;
  ariaControls?: string;
  ariaDisabled?: string;
}

export interface MenuKeyboardNavigation {
  enabled: boolean;
  openKeys: KeyboardKeyList;
  openToLastKeys: KeyboardKeyList;
  nextKeys: KeyboardKeyList;
  previousKeys: KeyboardKeyList;
  firstKeys: KeyboardKeyList;
  lastKeys: KeyboardKeyList;
  selectKeys: KeyboardKeyList;
  closeKeys: KeyboardKeyList;
  dismissKeys: KeyboardKeyList;
  loop: boolean;
  preventDefault: boolean;
  initialActiveItemId?: string;
  activeItemClass: string;
}

export const DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS =
  'menu-keyboard__item--active';

export const DEFAULT_MENU_KEYBOARD_NAVIGATION: MenuKeyboardNavigation = {
  enabled: true,
  openKeys: ['ArrowDown', 'ArrowUp', 'Enter', ' '],
  openToLastKeys: ['ArrowUp'],
  nextKeys: ['ArrowDown'],
  previousKeys: ['ArrowUp'],
  firstKeys: ['Home'],
  lastKeys: ['End'],
  selectKeys: ['Enter', ' '],
  closeKeys: ['Escape'],
  dismissKeys: ['Tab'],
  loop: true,
  preventDefault: true,
  activeItemClass: DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS,
};

export type MenuKeyboardNavigationInput =
  | boolean
  | Partial<MenuKeyboardNavigation>
  | undefined;

export interface MenuKeyboardConfig<T extends MenuKeyboardItem> {
  getItems: () => readonly T[];
  getItemId?: (item: T) => string | undefined;
  onSelect: (item: T) => void;
  onOpen: () => void;
  onClose: (restoreFocus: boolean) => void;
  keyboardNavigation?:
    | MenuKeyboardNavigationInput
    | (() => MenuKeyboardNavigationInput);
}
