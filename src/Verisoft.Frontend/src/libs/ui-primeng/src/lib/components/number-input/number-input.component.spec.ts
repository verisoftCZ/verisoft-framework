import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { of } from 'rxjs';
import { NumberInputComponent } from './number-input.component';

describe('NumberInputComponent', () => {
  let component: NumberInputComponent;
  let fixture: ComponentFixture<NumberInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NumberInputComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NumberInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { Component } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

@Component({
  imports: [NumberInputComponent, ReactiveFormsModule],
  template: `<v-number-input [formControl]="ctrl" [clearable]="clearable" [decimals]="2" />`,
})
class ReactiveHostComponent {
  ctrl = new FormControl<number | null>(5);
  clearable = true;
}

@Component({
  imports: [NumberInputComponent, FormsModule],
  template: `<v-number-input [(ngModel)]="value" [disabled]="disabled" [decimals]="2" [min]="0" />`,
})
class NgModelHostComponent {
  value: number | null = 5;
  disabled = true;
}

describe('NumberInputComponent clear icon', () => {
  const providers = [
    { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } },
  ];
  const clearIcon = (f: ComponentFixture<unknown>) =>
    f.debugElement.query(By.css('.p-inputnumber-clear-icon'));
  const input = (f: ComponentFixture<unknown>) =>
    f.debugElement.query(By.css('input')).nativeElement as HTMLInputElement;

  describe('reactive', () => {
    let fixture: ComponentFixture<ReactiveHostComponent>;
    let host: ReactiveHostComponent;

    beforeEach(async () => {
      await TestBed.configureTestingModule({
        imports: [ReactiveHostComponent],
        providers,
      }).compileComponents();
      fixture = TestBed.createComponent(ReactiveHostComponent);
      host = fixture.componentInstance;
    });

    it('shows clear icon when enabled and has value', () => {
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeTruthy();
    });

    it('hides clear icon when initially disabled', () => {
      host.ctrl.disable();
      fixture.detectChanges();
      expect(input(fixture).disabled).toBe(true);
      expect(clearIcon(fixture)).toBeNull();
    });

    it('hides clear icon when disabled dynamically and shows it again when re-enabled', () => {
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeTruthy();

      host.ctrl.disable();
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeNull();
      expect(host.ctrl.value).toBe(5);

      host.ctrl.enable();
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeTruthy();
    });

    it('enabled clear click empties the value', () => {
      fixture.detectChanges();
      clearIcon(fixture).nativeElement.dispatchEvent(new Event('click'));
      fixture.detectChanges();
      expect(host.ctrl.value).toBeNull();
      expect(clearIcon(fixture)).toBeNull();
    });

    it('does not show clear icon when clearable is false', () => {
      host.clearable = false;
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeNull();
    });

    it('keeps value when disabled', () => {
      host.ctrl.disable();
      fixture.detectChanges();
      expect(host.ctrl.value).toBe(5);
      expect(input(fixture).value).toContain('5');
    });
  });

  describe('ngModel with static [disabled]="true"', () => {
    let fixture: ComponentFixture<NgModelHostComponent>;
    let host: NgModelHostComponent;

    beforeEach(async () => {
      await TestBed.configureTestingModule({
        imports: [NgModelHostComponent],
        providers,
      }).compileComponents();
      fixture = TestBed.createComponent(NgModelHostComponent);
      host = fixture.componentInstance;
    });

    it('hides clear icon when disabled', async () => {
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
      expect(input(fixture).disabled).toBe(true);
      expect(clearIcon(fixture)).toBeNull();
      expect(host.value).toBe(5);
    });

    it('toggles clear icon with dynamic [disabled]', async () => {
      host.disabled = false;
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeTruthy();

      host.disabled = true;
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
      expect(input(fixture).disabled).toBe(true);
      expect(clearIcon(fixture)).toBeNull();

      host.disabled = false;
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
      expect(clearIcon(fixture)).toBeTruthy();
    });
  });
});
