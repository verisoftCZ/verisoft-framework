import { signal } from '@angular/core';
import { filterEnabledItems } from '../core/linear-navigation.utils';
import { MenuKeyboardController } from './menu-keyboard.controller';
import { MenuKeyboardFocusA11y, MenuKeyboardItem, DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS } from './menu-keyboard.model';
import { MenuKeyboardPort } from './menu-keyboard-port.token';

export class MenuKeyboardFacade<T extends MenuKeyboardItem = MenuKeyboardItem> {
  private static nextListId = 0;

  readonly isOpen = signal(false);
  readonly focusedIndex = signal(-1);
  readonly focusRequestVersion = signal(0);
  readonly activeItemClass = signal(DEFAULT_MENU_KEYBOARD_ACTIVE_ITEM_CLASS);
  readonly listId = signal(`v-menu-kb-list-${MenuKeyboardFacade.nextListId++}`);

  private readonly actionableItems = signal<readonly T[]>([]);
  private triggerElement: HTMLElement | null = null;
  private listElement: HTMLElement | null = null;
  private readonly itemElements = new Map<T, HTMLElement>();
  private activeItemHandler: ((item: T) => void) | null = null;
  private triggerKeydownHandler: ((event: KeyboardEvent) => void) | null = null;
  private focusItemHandler:
    | ((host: HTMLElement, a11y?: MenuKeyboardFocusA11y) => void)
    | null = null;
  private syncTriggerHandler:
    | ((host: HTMLElement, a11y: MenuKeyboardFocusA11y) => void)
    | null = null;
  private syncItemHandler:
    | ((host: HTMLElement, a11y: MenuKeyboardFocusA11y) => void)
    | null = null;
  private portProvider: (() => MenuKeyboardPort | null) | null = null;

  setPortProvider(provider: () => MenuKeyboardPort | null): void {
    this.portProvider = provider;
  }

  setActiveItemHandler(handler: (item: T) => void): void {
    this.activeItemHandler = handler;
  }

  setTriggerKeydownHandler(handler: (event: KeyboardEvent) => void): void {
    this.triggerKeydownHandler = handler;
  }

  setFocusItemHandler(
    handler: (host: HTMLElement, a11y?: MenuKeyboardFocusA11y) => void,
  ): void {
    this.focusItemHandler = handler;
  }

  setSyncTriggerHandler(
    handler: (host: HTMLElement, a11y: MenuKeyboardFocusA11y) => void,
  ): void {
    this.syncTriggerHandler = handler;
  }

  setSyncItemHandler(
    handler: (host: HTMLElement, a11y: MenuKeyboardFocusA11y) => void,
  ): void {
    this.syncItemHandler = handler;
  }

  registerTrigger(element: HTMLElement): void {
    this.triggerElement = element;
  }

  unregisterTrigger(element: HTMLElement): void {
    if (this.triggerElement === element) {
      this.triggerElement = null;
    }
  }

  registerList(element: HTMLElement): void {
    this.listElement = element;
  }

  unregisterList(element: HTMLElement): void {
    if (this.listElement === element) {
      this.listElement = null;
    }
  }

  registerItem(item: T, element: HTMLElement): void {
    this.itemElements.set(item, element);
  }

  unregisterItem(item: T, element: HTMLElement): void {
    const registered = this.itemElements.get(item);
    if (registered === element) {
      this.itemElements.delete(item);
    }
  }

  unregisterItemElement(element: HTMLElement): void {
    for (const [item, registered] of this.itemElements.entries()) {
      if (registered === element) {
        this.itemElements.delete(item);
      }
    }
  }

  getTriggerElement(): HTMLElement | null {
    return this.triggerElement;
  }

  getListElement(): HTMLElement | null {
    return this.listElement;
  }

  publish(controller: MenuKeyboardController<T>, items: readonly T[]): void {
    const wasOpen = this.isOpen();
    const previousFocusedIndex = this.focusedIndex();
    const nextFocusedIndex = controller.focusedIndex;

    this.isOpen.set(controller.isOpen);
    this.focusedIndex.set(nextFocusedIndex);
    this.activeItemClass.set(controller.getActiveItemClass());
    this.actionableItems.set(filterEnabledItems(items));

    if (
      controller.isOpen &&
      wasOpen &&
      nextFocusedIndex >= 0 &&
      previousFocusedIndex !== nextFocusedIndex
    ) {
      this.focusRequestVersion.update((value) => value + 1);
    }
  }

  isItemActive(item: T): boolean {
    if (!this.isOpen() || this.focusedIndex() < 0) {
      return false;
    }

    const items = this.actionableItems();
    return items[this.focusedIndex()] === item;
  }

  getItemTabIndex(item: T): number {
    return this.isItemActive(item) ? 0 : -1;
  }

  requestActiveItem(item: T): void {
    this.activeItemHandler?.(item);
  }

  requestTriggerKeydown(event: KeyboardEvent): void {
    this.triggerKeydownHandler?.(event);
  }

  getActiveItemElement(): HTMLElement | null {
    const items = this.actionableItems();
    const index = this.focusedIndex();

    if (!this.isOpen() || index < 0 || index >= items.length) {
      return null;
    }

    return this.itemElements.get(items[index]) ?? null;
  }

  requestItemFocus(host: HTMLElement, a11y?: MenuKeyboardFocusA11y): void {
    if (this.focusItemHandler) {
      this.focusItemHandler(host, a11y);
      return;
    }

    host.focus({ preventScroll: true });
  }

  requestTriggerAccessibilitySync(
    host: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    this.syncTriggerHandler?.(host, a11y);
  }

  requestItemAccessibilitySync(
    host: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    this.syncItemHandler?.(host, a11y);
  }

  resolveItemFromTarget(target: Node): T | null {
    for (const [item, element] of this.itemElements.entries()) {
      if (element.contains(target)) {
        return item;
      }
    }

    return null;
  }

  getPort(): MenuKeyboardPort | null {
    return this.portProvider?.() ?? null;
  }
}
