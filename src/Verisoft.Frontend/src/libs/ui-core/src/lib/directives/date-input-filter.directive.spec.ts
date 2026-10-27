import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DateInputFilterDirective } from './date-input-filter.directive';

@Component({
  imports: [DateInputFilterDirective],
  template: `<div vDateInputFilter><input /></div>`,
})
class TestHostComponent {}

describe('DateInputFilterDirective', () => {
  let fixture: ComponentFixture<TestHostComponent>;
  let input: HTMLInputElement;

  const dispatch = (data: string) => {
    const event = new InputEvent('beforeinput', {
      data,
      bubbles: true,
      cancelable: true,
    });
    input.dispatchEvent(event);
    return event;
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestHostComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    fixture.detectChanges();
    input = fixture.nativeElement.querySelector('input');
  });

  it('should block letters', () => {
    expect(dispatch('a').defaultPrevented).toBe(true);
  });

  it('should block pasted text containing letters', () => {
    expect(dispatch('12.4.asd').defaultPrevented).toBe(true);
  });

  it('should allow digits and date separators', () => {
    for (const data of ['1', '.', '/', '-', ' ', ':', '23.04.2026']) {
      expect(dispatch(data).defaultPrevented).toBe(false);
    }
  });
});
