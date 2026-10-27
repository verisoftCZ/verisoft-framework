import { DestroyRef, Directive, ElementRef, inject } from '@angular/core';
import { MENU_KEYBOARD } from './menu-keyboard.token';

@Directive({
  selector: '[vMenuKeyboardList]',
  host: {
    role: 'menu',
    tabindex: '-1',
    '[id]': 'facade.listId()',
  },
})
export class MenuKeyboardListDirective {
  protected readonly facade = inject(MENU_KEYBOARD);
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    const element = this.elementRef.nativeElement;
    this.facade.registerList(element);

    this.destroyRef.onDestroy(() => {
      this.facade.unregisterList(element);
    });
  }
}
