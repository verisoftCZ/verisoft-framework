import { Directive, ElementRef, inject, Injector } from '@angular/core';
import { DropdownEvent } from '@gov-design-system-ce/components';
import {
  MENU_KEYBOARD_PORT,
  MenuKeyboardDirective,
  MenuKeyboardFocusA11y,
  MenuKeyboardPort,
} from '@verisoft/ui-core';
import {
  applyGovButtonAccessibility,
  focusGovButtonHost,
} from '../components/button/gov-button-focus.util';
import { GovDropdownElement } from './gov-dropdown.element';

@Directive({
  selector: 'gov-dropdown[vGovDropdownKeyboard]',
  host: {
    '(gov-change)': 'onGovChange($event)',
  },
  providers: [
    { provide: MENU_KEYBOARD_PORT, useExisting: GovDropdownKeyboardDirective },
  ],
})
export class GovDropdownKeyboardDirective implements MenuKeyboardPort {
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly injector = inject(Injector);

  onGovChange(event: Event): void {
    const detail = (event as CustomEvent<DropdownEvent>).detail;
    const host = this.getHost();

    host?.handleOpenChange(detail.open);

    if (detail.open && host?.isOpen()) {
      setTimeout(() => {
        if (host.isOpen()) {
          host.focusActiveItem();
        }
      }, 0);
    }
  }

  async open(): Promise<boolean> {
    const host = this.getHost();
    if (host && !host.hasActionableItems()) {
      return false;
    }

    const dropdown = this.getGovDropdown();

    if (dropdown) {
      const opened = await this.setGovOpen(dropdown, true);
      if (opened) {
        host?.handleOpenChange(true);
      }
      return opened;
    }

    this.clickTrigger();
    host?.handleOpenChange(true);
    return true;
  }

  async close(restoreFocus: boolean): Promise<void> {
    const host = this.getHost();

    if (restoreFocus && host) {
      const trigger = host.getTriggerElement();
      if (trigger) {
        this.focusTrigger(trigger, {
          ariaHaspopup: 'menu',
          ariaExpanded: 'false',
          ariaControls: host.listId(),
        });
      }
    }

    const dropdown = this.getGovDropdown();
    let closed = false;

    if (dropdown) {
      closed = await this.setGovOpen(dropdown, false);
    } else {
      this.clickTrigger();
      closed = true;
    }

    if (closed) {
      host?.handleOpenChange(false);
    }
  }

  focusTrigger(triggerHost: HTMLElement, a11y?: MenuKeyboardFocusA11y): void {
    focusGovButtonHost(triggerHost, a11y);
  }

  focusItem(itemHost: HTMLElement, a11y?: MenuKeyboardFocusA11y): void {
    focusGovButtonHost(itemHost, a11y);
  }

  applyTriggerAccessibility(
    triggerHost: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    applyGovButtonAccessibility(triggerHost, a11y);
  }

  applyItemAccessibility(
    itemHost: HTMLElement,
    a11y: MenuKeyboardFocusA11y,
  ): void {
    applyGovButtonAccessibility(itemHost, a11y);
  }

  private async setGovOpen(
    dropdown: GovDropdownElement,
    open: boolean,
  ): Promise<boolean> {
    try {
      const isOpen = await dropdown.getState();
      if (isOpen !== open) {
        await dropdown.toggle();
      }

      return true;
    } catch {
      if (open) {
        this.clickTrigger();
      }

      return open;
    }
  }

  private clickTrigger(): void {
    const trigger = this.getHost()?.getTriggerElement();
    const govButton = trigger?.querySelector('gov-button');

    if (govButton instanceof HTMLElement) {
      govButton.click();
      return;
    }

    trigger?.click();
  }

  private getGovDropdown(): GovDropdownElement | undefined {
    const element = this.elementRef.nativeElement;
    if (typeof (element as GovDropdownElement).getState !== 'function') {
      return undefined;
    }

    return element as GovDropdownElement;
  }

  private getHost(): MenuKeyboardDirective | null {
    return this.injector.get(MenuKeyboardDirective, null, { optional: true });
  }
}
