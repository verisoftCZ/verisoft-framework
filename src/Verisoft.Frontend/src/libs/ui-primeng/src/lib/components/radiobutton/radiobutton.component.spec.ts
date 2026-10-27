import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { of } from 'rxjs';
import { RadioButtonComponent } from './radiobutton.component';

describe('RadioButtonComponent', () => {
  let component: RadioButtonComponent<any>;
  let fixture: ComponentFixture<RadioButtonComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RadioButtonComponent],
      providers: [
        { provide: ERROR_PROVIDER_TOKEN, useValue: { mapError: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RadioButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
