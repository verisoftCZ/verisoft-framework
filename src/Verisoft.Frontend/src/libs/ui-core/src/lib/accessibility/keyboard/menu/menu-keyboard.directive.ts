import {
  DestroyRef,
  Directive,
  ElementRef,
  inject,
  Injector,
  input,
  isDevMode,
  output,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { MenuKeyboardController } from './menu-keyboard.controller';
import {
  MenuKeyboardConfig,
  MenuKeyboardFocusA11y,
  MenuKeyboardItem,
  MenuKeyboardNavigation,
  MenuKeyboardNavigationInput,
} from './menu-keyboard.model';
import {
  MENU_KEYBOARD_PORT,
  MenuKeyboardPort,
} from './menu-keyboard-port.token';
import { assertNever } from '../core/keyboard-action.model';
import {
  MenuKeyboardPortIntent,
  MenuKeyboardPortOperationState,
} from './menu-keyboard.port-state';
import { MenuKeyboardFacade } from './menu-keyboard.facade';
import { MENU_KEYBOARD } from './menu-keyboard.token';

@Directive({
  selector: '[vMenuKeyboard]',
  exportAs: 'menuKeyboard',
  providers: [
    MenuKeyboardFacade,
    { provide: MENU_KEYBOARD, useExisting: MenuKeyboardFacade },
  ],
})
export class MenuKeyboardDirective<
  T extends MenuKeyboardItem = MenuKeyboardItem,
> {
  readonly menuKeyboardItems = input<readonly T[]>([]);
  readonly menuKeyboardNavigation =
    input<MenuKeyboardNavigationInput>(undefined);
  readonly menuKeyboardEnableWhenUnset = input(false);
  readonly menuKeyboardDefaults = input<Partial<MenuKeyboardNavigation>>({});
  readonly menuKeyboardItemId = input<(item: T) => string | undefined>(
    (item) => (item as { id?: string }).id,
  );
  readonly menuKeyboardSelect = output<T>();

  private readonly facade = inject(MenuKeyboardFacade<T>);
  readonly isOpen = this.facade.isOpen;
  readonly focusedIndex = this.facade.focusedIndex;
  readonly activeItemClass = this.facade.activeItemClass;
  readonly listId = this.facade.listId;

  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly document = inject(DOCUMENT);
  private readonly injector = inject(Injector);
  private readonly destroyRef = inject(DestroyRef);

  private readonly onDocumentKeydown = (event: KeyboardEvent) =>
    this.handleDocumentKeydown(event);

  private portOperationState: MenuKeyboardPortOperationState = 'idle';
  private pendingPortIntent: MenuKeyboardPortIntent | null = null;

  private readonly controller = new MenuKeyboardController<T>({
    getItems: (): readonly T[] => this.menuKeyboardItems(),
    getItemId: (item: T): string | undefined => this.menuKeyboardItemId()(item),
    keyboardNavigation: (): MenuKeyboardNavigationInput =>
      this.resolveNavigation(),
    onSelect: (item: T): void => this.menuKeyboardSelect.emit(item),
    onOpen: (): void => {
      this.schedulePortIntent({ type: 'open' });
    },
    onClose: (restoreFocus: boolean): void => {
      this.close(restoreFocus);
    },
  } satisfies MenuKeyboardConfig<T>);

  constructor() {
    this.facade.setPortProvider(() => this.getPort());

    this.facade.setActiveItemHandler((item) => {
      this.controller.setActiveItem(item);
      this.publishState();
    });

    this.facade.setTriggerKeydownHandler((event) => {
      this.handleTriggerKeydown(event);
    });

    this.facade.setFocusItemHandler((host, a11y) => {
      this.focusItemHost(host, a11y);
    });

    this.facade.setSyncTriggerHandler((host, a11y) => {
      this.applyTriggerAccessibility(host, a11y);
    });

    this.facade.setSyncItemHandler((host, a11y) => {
      this.applyItemAccessibility(host, a11y);
    });

    this.destroyRef.onDestroy(() => {
      this.facade.setTriggerKeydownHandler(() => undefined);
      this.facade.setFocusItemHandler(() => undefined);
      this.facade.setSyncTriggerHandler(() => undefined);
      this.facade.setSyncItemHandler(() => undefined);
      this.facade.setPortProvider(() => null);
      this.unbindDocumentKeydown();
    });
  }

  handleOpenChange(open: boolean): void {
    this.controller.handleOpenChange(open);

    if (open && !this.controller.isOpen) {
      void this.schedulePortIntent({ type: 'close', restoreFocus: false });
    }

    if (this.controller.isOpen) {
      this.bindDocumentKeydown();
    } else {
      this.unbindDocumentKeydown();
    }

    this.publishState();
  }

  close(restoreFocus = false): void {
    if (restoreFocus) {
      this.focusTrigger({ ariaExpanded: 'false' });
    }

    this.schedulePortIntent({ type: 'close', restoreFocus: false });
  }

  focusTrigger(a11yOverrides?: Partial<MenuKeyboardFocusA11y>): void {
    const trigger = this.facade.getTriggerElement();
    if (!trigger) {
      return;
    }

    const a11y: MenuKeyboardFocusA11y = {
      ariaHaspopup: 'menu',
      ariaExpanded: String(this.facade.isOpen()),
      ariaControls: this.facade.listId(),
      ...a11yOverrides,
    };

    const port = this.getPort();
    if (port?.focusTrigger) {
      port.focusTrigger(trigger, a11y);
      return;
    }

    trigger.focus({ preventScroll: true });
  }

  focusActiveItem(): void {
    if (!this.controller.isOpen || this.controller.focusedIndex < 0) {
      return;
    }

    const items = this.controller.actionableItems;
    const item = items[this.controller.focusedIndex];
    const host = this.facade.getActiveItemElement();

    if (!item || !host) {
      return;
    }

    this.focusItemHost(host, this.buildItemFocusA11y(item));
  }

  getTriggerElement(): HTMLElement | undefined {
    return this.facade.getTriggerElement() ?? undefined;
  }

  getListElement(): HTMLElement | undefined {
    return this.facade.getListElement() ?? undefined;
  }

  isItemActive(item: T): boolean {
    return this.facade.isItemActive(item);
  }

  getItemTabIndex(item: T): number {
    return this.facade.getItemTabIndex(item);
  }

  hasActionableItems(): boolean {
    return this.controller.actionableItems.length > 0;
  }

  private handleTriggerKeydown(event: KeyboardEvent): void {
    if (this.controller.isOpen || !this.controller.isNavigationEnabled()) {
      return;
    }

    this.controller.onTriggerKeydown(event);
    this.publishState();
  }

  private handleDocumentKeydown(event: KeyboardEvent): void {
    if (!this.controller.isOpen || !this.shouldHandleDocumentKeydown(event)) {
      return;
    }

    this.controller.onMenuKeydown(event);
    this.publishState();
  }

  private shouldHandleDocumentKeydown(event: KeyboardEvent): boolean {
    const host = this.elementRef.nativeElement;
    const listElement = this.facade.getListElement();
    const path = event.composedPath();

    if (path.includes(host)) {
      return true;
    }

    return !!(listElement && path.includes(listElement));
  }

  private bindDocumentKeydown(): void {
    this.document.removeEventListener('keydown', this.onDocumentKeydown, true);
    this.document.addEventListener('keydown', this.onDocumentKeydown, true);
  }

  private unbindDocumentKeydown(): void {
    this.document.removeEventListener('keydown', this.onDocumentKeydown, true);
  }

  private focusItemHost(host: HTMLElement, a11y?: MenuKeyboardFocusA11y): void {
    const resolvedA11y =
      a11y ??
      (() => {
        const item = this.facade.resolveItemFromTarget(host);
        return item ? this.buildItemFocusA11y(item) : undefined;
      })();

    const port = this.getPort();
    if (port?.focusItem) {
      port.focusItem(host, resolvedA11y);
      return;
    }

    host.focus({ preventScroll: true });
  }

  private applyTriggerAccessibility(
    host: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    this.getPort()?.applyTriggerAccessibility?.(host, a11y);
  }

  private applyItemAccessibility(
    host: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    this.getPort()?.applyItemAccessibility?.(host, a11y);
  }

  private buildItemFocusA11y(item: T): MenuKeyboardFocusA11y {
    return {
      role: 'menuitem',
      tabIndex: this.getItemTabIndex(item),
      ariaDisabled: String(item.disabled ?? false),
    };
  }

  private publishState(): void {
    this.facade.publish(this.controller, this.menuKeyboardItems());
  }

  private schedulePortIntent(intent: MenuKeyboardPortIntent): void {
    if (this.portOperationState === 'idle') {
      void this.executePortIntent(intent);
      return;
    }

    this.pendingPortIntent = intent;
  }

  private async executePortIntent(
    intent: MenuKeyboardPortIntent,
  ): Promise<void> {
    switch (intent.type) {
      case 'open':
        await this.openViaPort();
        return;
      case 'close':
        await this.closeViaPort(intent.restoreFocus);
        return;
      default:
        assertNever(intent);
    }
  }

  private async drainPendingPortIntent(): Promise<void> {
    const intent = this.pendingPortIntent;
    this.pendingPortIntent = null;

    if (!intent) {
      return;
    }

    await this.executePortIntent(intent);
  }

  private async openViaPort(): Promise<void> {
    const port = this.getPort();
    if (!port) {
      this.controller.cancelPendingOpen();
      this.publishState();
      return;
    }

    this.portOperationState = 'opening';
    try {
      const opened = await port.open();
      if (opened === false) {
        this.controller.cancelPendingOpen();
      }
    } catch (error) {
      this.controller.cancelPendingOpen();
      this.reportPortError('open', error);
    } finally {
      this.portOperationState = 'idle';
      this.publishState();
      await this.drainPendingPortIntent();
    }
  }

  private async closeViaPort(restoreFocus: boolean): Promise<void> {
    const port = this.getPort();
    if (!port) {
      return;
    }

    this.portOperationState = 'closing';
    try {
      await port.close(restoreFocus);
    } catch (error) {
      this.reportPortError('close', error);
    } finally {
      this.portOperationState = 'idle';
      this.publishState();
      await this.drainPendingPortIntent();
    }
  }

  private reportPortError(operation: 'open' | 'close', error: unknown): void {
    if (isDevMode()) {
      console.warn(`[vMenuKeyboard] Port ${operation} failed.`, error);
    }
  }

  private getPort(): MenuKeyboardPort | null {
    return this.injector.get(MENU_KEYBOARD_PORT, null, { optional: true });
  }

  private resolveNavigation(): MenuKeyboardNavigationInput {
    const input = this.menuKeyboardNavigation();
    const defaults = this.menuKeyboardDefaults();

    if (input === false) {
      return false;
    }

    if (input === undefined) {
      if (!this.menuKeyboardEnableWhenUnset()) {
        return undefined;
      }

      return Object.keys(defaults).length ? defaults : true;
    }

    if (input === true) {
      return Object.keys(defaults).length ? defaults : true;
    }

    return {
      ...defaults,
      ...input,
    };
  }
}
