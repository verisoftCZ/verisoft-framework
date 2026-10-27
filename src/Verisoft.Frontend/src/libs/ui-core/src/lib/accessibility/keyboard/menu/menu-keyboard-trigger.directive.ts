import { DestroyRef, Directive, ElementRef, effect, inject } from '@angular/core';
import { MENU_KEYBOARD } from './menu-keyboard.token';

@Directive({
  selector: '[vMenuKeyboardTrigger]',
  host: {
    'aria-haspopup': 'menu',
    '[attr.aria-expanded]': 'facade.isOpen()',
    '[attr.aria-controls]': 'facade.listId()',
    '(keydown)': 'onKeydown($event)',
  },
})
export class MenuKeyboardTriggerDirective {
  protected readonly facade = inject(MENU_KEYBOARD);
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    const element = this.elementRef.nativeElement;
    this.facade.registerTrigger(element);

    effect(() => {
      this.facade.requestTriggerAccessibilitySync(element, {
        ariaHaspopup: 'menu',
        ariaExpanded: String(this.facade.isOpen()),
        ariaControls: this.facade.listId(),
      });
    });

    this.destroyRef.onDestroy(() => {
      this.facade.unregisterTrigger(element);
    });
  }

  onKeydown(event: KeyboardEvent): void {
    this.facade.requestTriggerKeydown(event);
  }
}
