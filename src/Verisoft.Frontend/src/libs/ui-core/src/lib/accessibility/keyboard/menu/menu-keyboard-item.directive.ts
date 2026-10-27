import {
  DestroyRef,
  Directive,
  ElementRef,
  effect,
  inject,
  input,
} from '@angular/core';
import { MENU_KEYBOARD } from './menu-keyboard.token';
import { MenuKeyboardItem } from './menu-keyboard.model';

@Directive({
  selector: '[vMenuKeyboardItem]',
  host: {
    role: 'menuitem',
    '[attr.tabindex]': 'facade.getItemTabIndex(item())',
    '[attr.aria-disabled]': 'item().disabled ?? false',
    '(mouseenter)': 'onMouseEnter()',
  },
})
export class MenuKeyboardItemDirective<
  T extends MenuKeyboardItem = MenuKeyboardItem,
> {
  readonly item = input.required<T>({ alias: 'menuKeyboardItem' });

  protected readonly facade = inject(MENU_KEYBOARD);
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    const element = this.elementRef.nativeElement;

    effect((onCleanup) => {
      const menuItem = this.item();

      this.facade.registerItem(menuItem, element);

      onCleanup(() => {
        this.facade.unregisterItem(menuItem, element);
      });
    });

    effect(() => {
      const activeClass = this.facade.activeItemClass();
      const isActive = this.facade.isItemActive(this.item());
      element.classList.toggle(activeClass, isActive);
    });

    effect(() => {
      this.facade.isOpen();
      this.facade.focusedIndex();

      this.facade.requestItemAccessibilitySync(element, {
        role: 'menuitem',
        tabIndex: this.facade.getItemTabIndex(this.item()),
        ariaDisabled: String(this.item().disabled ?? false),
      });
    });

    effect(() => {
      this.facade.focusRequestVersion();
      if (!this.facade.isOpen() || !this.facade.isItemActive(this.item())) {
        return;
      }

      this.facade.requestItemFocus(element, {
        role: 'menuitem',
        tabIndex: this.facade.getItemTabIndex(this.item()),
        ariaDisabled: String(this.item().disabled ?? false),
      });
    });

    this.destroyRef.onDestroy(() => {
      this.facade.unregisterItemElement(element);
    });
  }

  onMouseEnter(): void {
    const menuItem = this.item();
    if (menuItem.disabled) {
      return;
    }

    this.facade.requestActiveItem(menuItem);
  }
}
