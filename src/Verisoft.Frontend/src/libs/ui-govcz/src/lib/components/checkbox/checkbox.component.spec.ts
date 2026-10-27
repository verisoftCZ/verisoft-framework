import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';
import { CheckboxComponent } from './checkbox.component';

describe('CheckboxComponent', () => {
  let component: CheckboxComponent;
  let fixture: ComponentFixture<CheckboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckboxComponent, TranslateModule.forRoot()],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

@Component({
  standalone: true,
  imports: [CheckboxComponent, ReactiveFormsModule, TranslateModule],
  template: `<v-checkbox [formControl]="control" [indeterminate]="indeterminate"></v-checkbox>`,
})
class ReactiveHostComponent {
  control = new FormControl<boolean | null>(null);
  indeterminate = false;
}

describe('CheckboxComponent value coercion', () => {
  let fixture: ComponentFixture<ReactiveHostComponent>;
  let host: ReactiveHostComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveHostComponent, TranslateModule.forRoot()],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ReactiveHostComponent);
    host = fixture.componentInstance;
  });

  it('coerces a null control value to false on init', async () => {
    fixture.detectChanges();
    await fixture.whenStable();

    expect(host.control.value).toBe(false);
  });

  it('coerces a later null patch to false when not indeterminate', () => {
    fixture.detectChanges();

    host.control.setValue(null);

    expect(host.control.value).toBe(false);
  });

  it('leaves the value untouched while indeterminate', () => {
    host.indeterminate = true;
    fixture.detectChanges();

    host.control.setValue(null);

    expect(host.control.value).toBeNull();
  });
});

@Component({
  standalone: true,
  imports: [CheckboxComponent, FormsModule, TranslateModule],
  template: `<v-checkbox [(ngModel)]="value" [indeterminate]="indeterminate"></v-checkbox>`,
})
class TemplateDrivenHostComponent {
  value: boolean | null = null;
  indeterminate = false;
}

describe('CheckboxComponent value coercion (template-driven forms)', () => {
  let fixture: ComponentFixture<TemplateDrivenHostComponent>;
  let host: TemplateDrivenHostComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateDrivenHostComponent, TranslateModule.forRoot()],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TemplateDrivenHostComponent);
    host = fixture.componentInstance;
  });

  it('resolves a null ngModel value to false when not indeterminate', async () => {
    fixture.detectChanges();
    await fixture.whenStable();

    expect(host.value).toBe(false);
  });

  it('leaves the ngModel value untouched while indeterminate', async () => {
    host.indeterminate = true;
    fixture.detectChanges();
    await fixture.whenStable();

    expect(host.value).toBeNull();
  });
});
