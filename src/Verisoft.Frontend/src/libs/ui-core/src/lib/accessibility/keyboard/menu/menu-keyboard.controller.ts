import {
  MenuKeyboardConfig,
  MenuKeyboardItem,
  MenuKeyboardNavigation,
  DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS,
} from './menu-keyboard.model';
import { assertNever } from '../core/keyboard-action.model';
import {
  MenuOpenKeyboardAction,
  MenuTriggerAction,
} from './menu-keyboard-action.model';
import {
  resolveMenuKeyboardAction,
  resolveMenuTriggerAction,
} from './menu-keyboard-bindings.utils';
import {
  filterEnabledItems,
  moveLinearIndex,
  resolveInitialActiveIndex,
} from '../core/linear-navigation.utils';
import { suppressKeyboardEvent } from '../core/keyboard.utils';
import { resolveMenuKeyboardNavigation } from './menu-keyboard.utils';

export class MenuKeyboardController<T extends MenuKeyboardItem> {
  focusedIndex = -1;
  isOpen = false;

  private keyboardPresetFocus = false;

  constructor(private readonly config: MenuKeyboardConfig<T>) {}

  get actionableItems(): readonly T[] {
    return filterEnabledItems(this.config.getItems());
  }

  isNavigationEnabled(): boolean {
    return this.resolveNavigation() !== null;
  }

  getActiveItemClass(): string {
    return (
      this.resolveNavigation()?.activeItemClass ??
      DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS
    );
  }

  onTriggerKeydown(event: KeyboardEvent): void {
    const navigation = this.resolveNavigation();
    if (!navigation || this.isOpen) {
      return;
    }

    const action = resolveMenuTriggerAction(event, navigation);
    if (!action) {
      return;
    }

    const items = this.actionableItems;
    if (!items.length) {
      return;
    }

    suppressKeyboardEvent(event, navigation.preventDefault);
    this.applyTriggerAction(action, items, navigation);
    this.keyboardPresetFocus = true;
    this.config.onOpen();
  }

  onMenuKeydown(event: KeyboardEvent): void {
    const navigation = this.resolveNavigation();
    if (!navigation || !this.isOpen) {
      return;
    }

    const items = this.actionableItems;
    if (!items.length) {
      return;
    }

    const action = resolveMenuKeyboardAction(event, navigation);
    if (!action) {
      return;
    }

    this.applyOpenMenuAction(action, event, navigation, items);
  }

  handleOpenChange(open: boolean): void {
    if (!open) {
      this.isOpen = false;
      this.focusedIndex = -1;
      this.keyboardPresetFocus = false;
      return;
    }

    const wasOpen = this.isOpen;
    this.isOpen = true;
    const navigation = this.resolveNavigation();
    const items = this.actionableItems;

    if (!items.length) {
      this.isOpen = false;
      this.focusedIndex = -1;
      this.keyboardPresetFocus = false;
      return;
    }

    if (!wasOpen) {
      if (!this.keyboardPresetFocus) {
        this.focusedIndex = navigation
          ? this.resolveInitialActiveIndex(navigation)
          : 0;
      }

      if (this.focusedIndex < 0 || this.focusedIndex >= items.length) {
        this.focusedIndex = 0;
      }
    }

    this.keyboardPresetFocus = false;
  }

  cancelPendingOpen(): void {
    this.keyboardPresetFocus = false;
    this.focusedIndex = -1;
  }

  close(restoreFocus: boolean): void {
    if (!this.isOpen) {
      return;
    }

    this.config.onClose(restoreFocus);
  }

  setActiveItem(item: T): void {
    if (!this.isOpen) {
      return;
    }

    const index = this.actionableItems.indexOf(item);
    if (index >= 0) {
      this.focusedIndex = index;
    }
  }

  isItemFocused(item: T): boolean {
    if (!this.isOpen || this.focusedIndex < 0) {
      return false;
    }

    const focusedItem = this.actionableItems[this.focusedIndex];
    return focusedItem === item;
  }

  getItemTabIndex(item: T): number {
    return this.isItemFocused(item) ? 0 : -1;
  }

  private applyTriggerAction(
    action: MenuTriggerAction,
    items: readonly T[],
    navigation: MenuKeyboardNavigation,
  ): void {
    switch (action) {
      case 'openToLast':
        this.focusedIndex = items.length - 1;
        return;
      case 'open':
        this.focusedIndex = this.resolveInitialActiveIndex(navigation);
        return;
      default:
        assertNever(action);
    }
  }

  private applyOpenMenuAction(
    action: MenuOpenKeyboardAction,
    event: KeyboardEvent,
    navigation: MenuKeyboardNavigation,
    items: readonly T[],
  ): void {
    switch (action) {
      case 'dismiss':
        this.close(false);
        return;
      case 'next':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.focusedIndex = moveLinearIndex(
          this.focusedIndex,
          1,
          items.length,
          navigation.loop,
        );
        return;
      case 'previous':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.focusedIndex = moveLinearIndex(
          this.focusedIndex,
          -1,
          items.length,
          navigation.loop,
        );
        return;
      case 'first':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.focusedIndex = 0;
        return;
      case 'last':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.focusedIndex = items.length - 1;
        return;
      case 'activate':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.activateFocused(items);
        return;
      case 'close':
        suppressKeyboardEvent(event, navigation.preventDefault);
        this.close(true);
        return;
      default:
        assertNever(action);
    }
  }

  private resolveNavigation(): MenuKeyboardNavigation | null {
    const input =
      typeof this.config.keyboardNavigation === 'function'
        ? this.config.keyboardNavigation()
        : this.config.keyboardNavigation;

    return resolveMenuKeyboardNavigation(input);
  }

  private resolveInitialActiveIndex(
    navigation: MenuKeyboardNavigation,
  ): number {
    return resolveInitialActiveIndex(
      this.actionableItems,
      (item) => this.getItemId(item),
      navigation.initialActiveItemId,
    );
  }

  private getItemId(item: T): string | undefined {
    if (this.config.getItemId) {
      return this.config.getItemId(item);
    }

    return (item as { id?: string }).id;
  }

  private activateFocused(items: readonly T[]): void {
    const item = items[this.focusedIndex];
    if (!item) {
      return;
    }

    this.config.onSelect(item);
    this.close(true);
  }
}
