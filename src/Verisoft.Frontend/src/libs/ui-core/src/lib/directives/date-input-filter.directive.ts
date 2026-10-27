import { Directive, HostListener } from '@angular/core';

const ALLOWED_DATE_INPUT = /^[0-9\s.,/:-]*$/;

@Directive({
  selector: '[vDateInputFilter]',
  standalone: true,
})
export class DateInputFilterDirective {
  @HostListener('beforeinput', ['$event'])
  onBeforeInput(event: Event): void {
    if (!(event instanceof InputEvent)) {
      return;
    }

    const target = event.composedPath()[0] ?? event.target;
    if (!(target instanceof HTMLInputElement)) {
      return;
    }

    const text = event.data ?? event.dataTransfer?.getData('text') ?? '';
    if (text && !ALLOWED_DATE_INPUT.test(text)) {
      event.preventDefault();
    }
  }
}
