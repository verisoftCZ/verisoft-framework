import { KeyboardKey, KeyboardKeyList } from './keyboard-key.model';

export function matchesKeyboardKey(
  event: KeyboardEvent,
  keys: KeyboardKeyList,
): boolean {
  if (!keys.length) {
    return false;
  }

  return keys.includes(event.key as KeyboardKey);
}

export function suppressKeyboardEvent(
  event: KeyboardEvent,
  preventDefault: boolean,
): void {
  if (!preventDefault) {
    return;
  }

  event.preventDefault();
  event.stopPropagation();
}
