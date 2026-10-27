import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GenericFieldComponent } from './generic-field.component';
import { NgControl, FormControl } from '@angular/forms';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';

describe('GenericFieldComponent', () => {
  let component: GenericFieldComponent<string>;
  let fixture: ComponentFixture<GenericFieldComponent<string>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericFieldComponent],
      providers: [
        { provide: NgControl, useValue: { control: new FormControl() } },
        { provide: ERROR_PROVIDER_TOKEN, useValue: {} }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(GenericFieldComponent<string>);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
