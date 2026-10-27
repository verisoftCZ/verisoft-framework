import { MenuKeyboardNavigation } from './menu-keyboard.model';
import {
  MenuOpenKeyboardAction,
  MenuTriggerAction,
} from './menu-keyboard-action.model';
import { matchesKeyboardKey } from '../core/keyboard.utils';

type MenuOpenActionBinding = {
  readonly keys: keyof Pick<
    MenuKeyboardNavigation,
    | 'dismissKeys'
    | 'nextKeys'
    | 'previousKeys'
    | 'firstKeys'
    | 'lastKeys'
    | 'selectKeys'
    | 'closeKeys'
  >;
  readonly action: MenuOpenKeyboardAction;
};

type MenuTriggerActionBinding = {
  readonly keys: keyof Pick<
    MenuKeyboardNavigation,
    'openToLastKeys' | 'openKeys'
  >;
  readonly action: MenuTriggerAction;
};

const MENU_OPEN_ACTION_CHECKS: readonly MenuOpenActionBinding[] = [
  { keys: 'dismissKeys', action: 'dismiss' },
  { keys: 'nextKeys', action: 'next' },
  { keys: 'previousKeys', action: 'previous' },
  { keys: 'firstKeys', action: 'first' },
  { keys: 'lastKeys', action: 'last' },
  { keys: 'selectKeys', action: 'activate' },
  { keys: 'closeKeys', action: 'close' },
] as const;

const MENU_TRIGGER_ACTION_CHECKS: readonly MenuTriggerActionBinding[] = [
  { keys: 'openToLastKeys', action: 'openToLast' },
  { keys: 'openKeys', action: 'open' },
] as const;

const MENU_TRIGGER_ACTION_ORDER: readonly MenuTriggerAction[] = [
  'openToLast',
  'open',
] as const;

export function resolveMenuTriggerAction(
  event: KeyboardEvent,
  navigation: MenuKeyboardNavigation,
): MenuTriggerAction | null {
  const matched = new Set<MenuTriggerAction>();

  for (const { keys, action } of MENU_TRIGGER_ACTION_CHECKS) {
    if (matchesKeyboardKey(event, navigation[keys])) {
      matched.add(action);
    }
  }

  for (const action of MENU_TRIGGER_ACTION_ORDER) {
    if (matched.has(action)) {
      return action;
    }
  }

  return null;
}

export function resolveMenuKeyboardAction(
  event: KeyboardEvent,
  navigation: MenuKeyboardNavigation,
): MenuOpenKeyboardAction | null {
  for (const { keys, action } of MENU_OPEN_ACTION_CHECKS) {
    if (matchesKeyboardKey(event, navigation[keys])) {
      return action;
    }
  }

  return null;
}
