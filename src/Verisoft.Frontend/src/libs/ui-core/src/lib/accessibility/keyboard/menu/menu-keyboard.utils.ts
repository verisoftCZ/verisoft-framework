import {
  DEFAULT_MENU_KEYBOARD_NAVIGATION,
  MenuKeyboardNavigation,
} from './menu-keyboard.model';

export function resolveMenuKeyboardNavigation(
  input: boolean | Partial<MenuKeyboardNavigation> | undefined,
): MenuKeyboardNavigation | null {
  if (input === undefined || input === false) {
    return null;
  }

  const resolved: MenuKeyboardNavigation = {
    ...DEFAULT_MENU_KEYBOARD_NAVIGATION,
    enabled: true,
    ...(input === true ? {} : input),
  };

  if (!resolved.enabled) {
    return null;
  }

  return resolved;
}
