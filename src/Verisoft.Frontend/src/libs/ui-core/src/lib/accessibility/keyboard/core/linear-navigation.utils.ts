export interface KeyboardNavigableItem {
  disabled?: boolean;
}

export function filterEnabledItems<T extends KeyboardNavigableItem>(
  items: readonly T[],
): readonly T[] {
  return items.filter((item) => !item.disabled);
}

export function moveLinearIndex(
  currentIndex: number,
  delta: number,
  length: number,
  loop: boolean,
): number {
  if (length === 0) {
    return -1;
  }

  if (currentIndex < 0) {
    return delta > 0 ? 0 : length - 1;
  }

  const next = currentIndex + delta;

  if (loop) {
    if (next >= length) {
      return 0;
    }

    if (next < 0) {
      return length - 1;
    }

    return next;
  }

  return Math.max(0, Math.min(length - 1, next));
}

export function resolveInitialActiveIndex<T>(
  items: readonly T[],
  getItemId: (item: T) => string | undefined,
  initialActiveItemId?: string,
): number {
  if (!items.length) {
    return -1;
  }

  if (initialActiveItemId) {
    const index = items.findIndex(
      (item) => getItemId(item) === initialActiveItemId,
    );
    if (index >= 0) {
      return index;
    }
  }

  return 0;
}

export function getRovingTabIndex(isActive: boolean): number {
  return isActive ? 0 : -1;
}
